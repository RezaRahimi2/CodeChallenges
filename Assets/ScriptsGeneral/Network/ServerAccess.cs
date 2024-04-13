using UnityEngine;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BestHTTP;

namespace Immersed.General
{
    /// <summary>
    /// Access to server with timeout
    /// </summary>
    public class ServerAccess : MonoBehaviour
    {
        public static ServerAccess Instance;
        public static long LastServerTime;
        static List<RequestBundle> BundleRequests = new List<RequestBundle>();


        private void Awake()
        {
            Instance = this;
        }

        static int BundleId;

        public void SendBatchRequest(RequestBundle bundleRequest)
        {
            BundleRequests.Add(bundleRequest);
            BundleRequests[BundleRequests.Count - 1].Id = BundleId;
            // Set request/s bundle id
            for (int i = 0; i < BundleRequests[BundleRequests.Count - 1].Requests.Count; i++)
            {
                BundleRequests[BundleRequests.Count - 1].Requests[i].BundleId = BundleId;
                if (BundleRequests[BundleRequests.Count - 1].Requests[i].isDone)
                {
                    if (BundleRequests[BundleRequests.Count - 1].Requests[i].retryState &&
                        BundleRequests[BundleRequests.Count - 1].Requests[i].forceRetry)
                    {
                        BundleRequests[BundleRequests.Count - 1].Requests[i].httpError = false;
                        BundleRequests[BundleRequests.Count - 1].Requests[i].timer = 0;
                        BundleRequests[BundleRequests.Count - 1].Requests[i]
                            .SetRequest(BundleRequests[BundleRequests.Count - 1].Requests[i].httpVerb);
                        HttpRequest(bundleRequest.Requests[i]);
                    }
                }
                else
                {
                    HttpRequest(bundleRequest.Requests[i]);
                }
            }

            BundleId++;
        }

        void OnResponse(Request response)
        {
            int bundleIndex = BundleRequests.FindIndex(x => x.Id == response.BundleId);
            if (BundleRequests[bundleIndex].ReqCount == 1)
            {
                for (int i = 0; i < BundleRequests[bundleIndex].Requests.Count; i++)
                {
                    if (BundleRequests[bundleIndex].Requests[i].httpError &&
                        BundleRequests[bundleIndex].Requests[i].forceRetry == true)
                    {
                        BundleRequests[bundleIndex].resStaus = false;
                        break;
                    }
                }

                if (BundleRequests[bundleIndex].resStaus)
                {
                    foreach (Request request in BundleRequests[bundleIndex].Requests)
                    {
                        request.SetResponse();
                        request.request.Dispose();
                    }

                    if (BundleRequests[bundleIndex].ChildRBC.ReqCount == 0)
                    {
                        RequestBundle tmpRBC = BundleRequests[bundleIndex];

                        while (tmpRBC.Requests.Count != 0)
                        {
                            if (tmpRBC.Callback != null)
                                tmpRBC.Callback(tmpRBC.resStaus, tmpRBC);
                            int rbcIndex = BundleRequests.FindIndex(x => x.Id == tmpRBC.Requests[0].BundleId);
                            tmpRBC = BundleRequests[rbcIndex].Parent;
                            BundleRequests.RemoveAt(rbcIndex);
                        }
                    }
                    else
                    {
                        Instance.SendBatchRequest(BundleRequests[bundleIndex].ChildRBC);
                    }
                }
                else
                {
                    if (BundleRequests[bundleIndex].Callback != null)
                        BundleRequests[bundleIndex]
                            .Callback(BundleRequests[bundleIndex].resStaus, BundleRequests[bundleIndex]);
                    BundleRequests.RemoveAt(bundleIndex);
                }
            }
            else
            {
                BundleRequests[bundleIndex].ReqCount--;
            }
        }

        public async void HttpRequest(Request request)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            try
            {
                request.request.State = HTTPRequestStates.Initial;
                var response = await request.request.GetHTTPResponseAsync(cancellationTokenSource.Token);
            }
            catch (AsyncHTTPException ex)
            {
                Debug.Log("Status Code: " + ex.StatusCode);
                Debug.Log("Message: " + ex.Message);
                Debug.Log("Content: " + ex.Content);
            }
            catch (TaskCanceledException)
            {
                Debug.LogWarning("Request Canceled!");
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            finally
            {
                cancellationTokenSource.Dispose();
            }

            OnResponse(request);
        }
    }
}