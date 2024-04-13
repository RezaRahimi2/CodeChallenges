using System;
using System.Collections.Generic;

namespace Immersed.General
{


//for send a list of request as a bundle and get result of them
    //created for get different parameters from a server with different requests 
    public class RequestBundle
    {
        public int Id;
        public int ReqCount;
        public Action<bool, RequestBundle> Callback;
        public List<Request> Requests;
        private RequestBundle childRBC;
        private RequestBundle parent;
        public bool resStaus = true;

        public RequestBundle ChildRBC
        {
            get
            {
                if (childRBC == null)
                {
                    childRBC = new RequestBundle();
                    childRBC.parent = this;
                }

                return childRBC;
            }

            set { childRBC = value; }
        }

        public RequestBundle Parent
        {
            get
            {
                if (parent == null)
                    parent = new RequestBundle();
                return parent;
            }

            set { parent = value; }
        }

        public RequestBundle()
        {
            Requests = new List<Request>();
        }

        public void Put<T>(RequestClass<T> item)
        {
            if (item != null)
            {
                Requests.Add(item);
                ReqCount++;
            }
        }

        public void PutIntoChild<T>(RequestClass<T> item)
        {
            if (item != null)
            {
                ChildRBC.Parent = this;
                ChildRBC.Requests.Add(item);
                ChildRBC.ReqCount++;
            }
        }

        public RequestClass<T> Get<T>(int index)
        {
            return Requests[index] as RequestClass<T>;
        }

        public RequestBundle(List<Request> requests, Action<bool, RequestBundle> callback)
        {
            Requests = new List<Request>();
            Requests = requests;
            Callback = callback;
            ReqCount = requests.Count;
        }

        public RequestBundle(Request request)
        {
            Requests = new List<Request>();
            Requests.Add(request);
            request.retryState = true;
            request.isDone = false;
            request.httpError = false;
            request.url = request.url;
            Callback = null;
            ReqCount++;
        }

        public RequestBundle(Request request, Action<bool, RequestBundle> callback)
        {
            Requests = new List<Request>();
            Requests.Add(request);
            Callback = callback;
            ReqCount++;
        }
    }
}