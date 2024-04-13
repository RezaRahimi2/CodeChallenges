using System;
using System.Collections.Generic;
using BestHTTP;

namespace Immersed.General
{
    public abstract class Request
    {
        public HTTPRequest request;

        //Used for retry failed request automatically
        public bool forceRetry = false;
        public bool retryState = false;
        public bool isDone = false;
        public float timer = 0;
        public bool httpError = false;

        // can use timeout for requests
        public float timeOut;
        public Dictionary<string, string> headers;
        public Dictionary<string, string> body;
        public string url;
        public int BundleId;
        public Verb httpVerb;
        protected int PageIndex = 0;

        protected int PageSize = 10;

        //for measuring the server response time 
        public long reqElapsedMilliseconds;

        public void AddHeader(string key, string value)
        {
            request.AddHeader(key, value);
        }

        public void AddBody(string key, string value)
        {
            request.AddField(key, value);
        }

        public void AddBinaryData(string key, byte[] value, string fileName)
        {
            request.AddBinaryData(key, value, fileName);
        }

        public abstract void SetResponse();
        public abstract void SetRequest(Verb method);
        public abstract Type ReqType { get; }
    }
}