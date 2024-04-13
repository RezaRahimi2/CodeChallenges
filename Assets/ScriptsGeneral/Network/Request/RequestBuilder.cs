using System;
using System.Collections.Generic;
using UnityEngine;

//using UnityHTTP;
namespace Immersed.General
{
    //for create request with Verb, body, headers
    public class RequestBuilder : IBuilder
    {
        public RequestBuilder(string url)
        {
            this.url = url;
        }

        public RequestBuilder()
        {
            url = DataManager.Data.HostUrl;
        }

        private string url;
        private Dictionary<string, string> body = new Dictionary<string, string>();
        private Dictionary<string, string> headers;
        private Verb httpVerb;
        private int size = 10;
        private int index = 0;

        public IBuilder AddUrlPart(string endPoint)
        {
            url += endPoint + "/";
            return this;
        }

        public IBuilder Method(Verb verb)
        {
            httpVerb = verb;
            return this;
        }

        public IBuilder AddBody(string key, string value)
        {
            body.Add(key, value);
            return this;
        }

        //for build a Request type with generic type
        public RequestClass<T> Build<T>(bool autoFire, bool forceRetry, APIs.GenericCallback<T> callback,
            Action<T> callBack = null)
        {
            if (httpVerb == Verb.GET)
                url = BuildGetUrl(url);

            RequestClass<T> request = new RequestClass<T>(httpVerb, url, body, headers, index, size)
            {
                reqCallback = new RequestCallback<T>()
            };
            
            request.forceRetry = forceRetry;
            request.reqCallback.Success = (status,response,responeText, message, code) =>
            {
                if (callback != null)
                {
                    if (callBack != null)
                        callBack(response);
                    callback(status, response,responeText, message, code);
                }
                else
                {
                    Debug.logger.Log(message);
                }
            };
            request.reqCallback.Error = (message,status) =>
            {
                if (callback != null)
                    callback(false,default(T) ,request.Response != null? request.Response.responseText:request.request.Exception.Message,message, status);
                else
                    Debug.logger.Log(message);
            };

            if (autoFire)
            {
                RequestBundle bndleReq = new RequestBundle(request);
                ServerAccess.Instance.SendBatchRequest(bndleReq);
                return null;
            }

            return request;
        }


        string BuildGetUrl(string oldUrl)
        {
            if (body.Count != 0)
            {
                if (oldUrl.EndsWith("/"))
                {
                    oldUrl.TrimEnd("/".ToCharArray());
                }

                oldUrl += "?";
                int i = 0;
            }

            return oldUrl;
        }

        public IBuilder AddHeader(string key, string value)
        {
            headers.Add(key, value);
            return this;
        }

        public IBuilder Page(int pgae, int size)
        {
            this.index = pgae;
            this.size = size;
            return this;
        }

        public IBuilder Page(int pgae)
        {
            this.index = pgae;
            this.size = 10;
            return this;
        }
    }

    //for declare different part of request
    public interface IBuilder
    {
        IBuilder Method(Verb verb);

        IBuilder AddHeader(string key, string value);

        IBuilder AddUrlPart(string urlPart);

        IBuilder AddBody(string key, string value);

        IBuilder Page(int pgae, int size);

        IBuilder Page(int pgae);
    }
}