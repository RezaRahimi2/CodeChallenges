using System;
using UnityEngine;


namespace Immersed.General
{
    public class OnResponseEvent<T,T1,T2>
    {
        public event Action<T,T1,T2> OnResponse;

        public void InvokEvent(T t,T1 t1,T2 t2)
        {
            OnResponse?.Invoke(t,t1,t2);
        }
    }
    //for get requests by RequestName enum and response
    public class RequestManager : Singleton<RequestManager>
    {
        public OnResponseEvent<string, UserModel, bool> OnResponseUserEvent = new OnResponseEvent<string, UserModel, bool>();
        
        [SerializeField] private APIs m_apis;

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            m_apis = new APIs(DataManager.Data);
        }

        public Request GetRequest(RequestNameWithApiVersion requestNameWithApiVersion)
        {
            Request requestClass = null;
            APIs.GenericCallback<UserModel> callback = ((status, response, responseText, message, code) =>
            {
                OnResponseUserEvent.InvokEvent(responseText,response, status);
            });;

            switch (requestNameWithApiVersion.RequestName)
            {
                case  RequestName.AnonymousLogin:
                {
                    requestClass =
                        m_apis.AnonymousLogin(requestNameWithApiVersion.APIVersion, callback,false);
                    break;
                }
                case RequestName.Register:
                {
                    requestClass =
                        m_apis.Register(requestNameWithApiVersion.APIVersion,callback, false);
                    break;
                }
                case RequestName.EmailLogin:
                    requestClass =
                        m_apis.EmailLogin(requestNameWithApiVersion.APIVersion,callback, false);
                    break;
                case RequestName.UpdateRegisteredUserData:
                    requestClass =
                        m_apis.UpdateRegisteredUserData(requestNameWithApiVersion.APIVersion,callback, false);
                    break;
                case RequestName.UpdateGuestUserData:
                    requestClass =
                        m_apis.UpdateGuestUserData(requestNameWithApiVersion.APIVersion,callback, false);
                    break;
                case RequestName.Purchase:
                    break;
                case  RequestName.GetDataWithAuthToken:
                    requestClass =
                        m_apis.GetUserDataWithAuthToken(requestNameWithApiVersion.APIVersion,callback, false);
                    break;
            }

            return requestClass;
        }
        
        //for get profile picture with url of user
        public Request GetProfilePicRequest(string profilePicURL,APIs.GenericCallback<byte[]> callback)
        {
            Request requestClass = m_apis.GetProfileImageWithURL(callback, profilePicURL,false,false);
            return requestClass;
        }
    }
}