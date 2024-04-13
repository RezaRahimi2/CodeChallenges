
using System;
using System.Collections.Generic;
using BestHTTP;
using Newtonsoft.Json;
using UnityEngine;

namespace Immersed.General
{
    public class RequestClass<T> : Request
    {
        public Response<T> Response;

        public RequestCallback<T> reqCallback;

        public override Type ReqType
        {
            get { return typeof(T); }
        }

        public T Value { get; set; }

        public override void SetRequest(Verb method)
        {
            if (method == Verb.POST)
            {
                request = new HTTPRequest(new Uri(url), HTTPMethods.Post);
            }
            else if (method == Verb.GET)
            {
                request = new HTTPRequest(new Uri(url), HTTPMethods.Get);
            }

            if (headers != null)
            {
                foreach (var item in headers)
                {
                    request.AddHeader(item.Key, item.Value);
                }
            }

            foreach (var keyValuePair in body)
            {
                request.AddField(keyValuePair.Key, keyValuePair.Value);
            }
        }

        public RequestClass(Verb method, string url, Dictionary<string, string> body,
            Dictionary<string, string> headers,
            int page, int size, float timeOut = 20)
        {
            this.httpVerb = method;
            this.timeOut = timeOut;
            this.body = body;
            this.headers = headers;

            if (url.EndsWith("/"))
                this.url = url.Remove(url.Length - 1);
            else
                this.url = url;

            this.PageSize = size;
            this.PageIndex = page;
            this.timer = 0;
            this.httpError = false;
            SetRequest(method);

            Debug.unityLogger.Log($"<color=#FFFF00>Request URL:{this.url}</color>");
        }

        public override void SetResponse()
        {
            isDone = true;
            if (httpError)
            {
                //Request.Dispose();
                string secretKey = headers.ContainsKey("SecretKey") ? headers["SecretKey"] : "No SecretKey";
                //GA_Helper.SendHttpRequestFailed(request.url, httpVerb == Verb.POST ? JsonConvert.SerializeObject(parameters) : request.url, request.method, secretKey, reqElapsedMilliseconds, headers["UniqueIdentify"]);
                //request = null;
                Debug.logger.Log("Timeout");
                retryState = true;
                //error = "timeout";
            }
            else if (request.Exception != null)
            {
                httpError = true;
                Debug.logger.Log(request.Exception);
                retryState = true;
            }
            else
            {
                Response = new Response<T>();

                if (typeof(T) == typeof(byte[]))
                {
                    Response.response = (T) Convert.ChangeType(request.Response.Data, typeof(byte[]));
                }
                else
                {
                    if (JsonExtensions.TryParseJson<UserModel>(request.Response.DataAsText,
                        DataManager.Data.UserModelSchemaJson) != null)
                        Response.response = JsonConvert.DeserializeObject<T>(request.Response.DataAsText);
                    else
                        Response.response = default(T);
                }

                Response.status = true;
                Response.message = request.Response.Message;
                Response.responseText = request.Response.DataAsText;
                Response.code = request.Response.StatusCode;


                if (request.Response.StatusCode == 200)
                {
                    Debug.unityLogger.Log(request.Response.DataAsText + " | " + (long) (timer * 1000));

                    try
                    {
                        // Server response
                        retryState = false;
                    }
                    catch (Exception e)
                    {
                        // Can send the http error to analytics
                        httpError = true;
                        retryState = true;
                        Debug.logger.Log("Catch error: | " + typeof(T) + " | " + e.Message);
                    }
                }
                else
                {
                    httpError = true;
                    Debug.logger.Log("Server side error: " + request.Response.StatusCode + " | " +
                                     request.Response.DataAsText);
                }
            }

            if (httpError)
            {
                reqCallback.Error("http error", -1);
            }
            else if (Response.isSuccessfull())
            {
                reqCallback.Success(Response.status, Response.response, Response.responseText, Response.message,
                    Response.code);
            }
            else
            {
                reqCallback.Error(Response.message, Response.code);
            }
        }
    }
}