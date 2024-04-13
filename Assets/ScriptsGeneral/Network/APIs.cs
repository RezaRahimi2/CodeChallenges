using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Immersed.General
{
    //declare all api request and generic callback 
    public class APIs
    {
        private DataScriptableObject m_data;

        public APIs(DataScriptableObject data)
        {
            m_data = data;
        }

        public delegate void GenericCallback<T>(bool status, T response, string responseText, string message,
            long code);

        [ExposeToAPIEditor]
        public RequestClass<UserModel> AnonymousLogin(APIVersion apiVersion,GenericCallback<UserModel> callback, bool autoFire = true,
            bool forceRetry = true,string deviceid = null)
        {
            RequestBuilder req = new RequestBuilder();
            req.Method(Verb.POST).AddUrlPart(apiVersion.ToString().ToLower()).AddUrlPart("auth").AddUrlPart("login").AddUrlPart("anonymous");
            
            if(!string.IsNullOrEmpty(deviceid))
                req.AddBody("deviceid", deviceid);
            
            return req.Build<UserModel>(autoFire, forceRetry, callback, (res) => { UpdateUserData(res as UserModel); });
        }

        [ExposeToAPIEditor]
        public RequestClass<UserModel> Register(APIVersion apiVersion,APIs.GenericCallback<UserModel> callback, bool autoFire = true,
            bool forceRetry = true,string email = null,string password = null, string nickname = null,string deviceid = null)
        {
            RequestBuilder req = new RequestBuilder();
            req.Method(Verb.POST).AddUrlPart(apiVersion.ToString().ToLower()).AddUrlPart("auth").AddUrlPart("register");
            
            if(!string.IsNullOrEmpty(email))
                req.AddBody("email", email);
            
            if(!string.IsNullOrEmpty(password))
                req.AddBody("password", password);
            
            if(!string.IsNullOrEmpty(nickname))
                req.AddBody("nickname", nickname);
            
            if(!string.IsNullOrEmpty(deviceid))
                req.AddBody("deviceid", deviceid);
            
            return req.Build<UserModel>(autoFire, forceRetry, callback, (res) => { UpdateUserData(res as UserModel); });
        }

        [ExposeToAPIEditor]
        public RequestClass<UserModel> EmailLogin(APIVersion apiVersion,GenericCallback<UserModel> callback, bool autoFire = true,
            bool forceRetry = true,string email = null,string password = null,string deviceid = null)
        {
            RequestBuilder req = new RequestBuilder();
            req.Method(Verb.POST).AddUrlPart(apiVersion.ToString().ToLower()).AddUrlPart("auth").AddUrlPart("login").AddUrlPart("email");
            
            if(!string.IsNullOrEmpty(deviceid))
                req.AddBody("deviceid", deviceid);
            
            if(!string.IsNullOrEmpty(email))
                req.AddBody("email", email);
            
            if(!string.IsNullOrEmpty(password))
                req.AddBody("password", password);
            
            return req.Build<UserModel>(autoFire, forceRetry, callback, (res) => { UpdateUserData(res as UserModel); });
        }
        
        public RequestClass<byte[]> GetProfileImageWithURL(GenericCallback<byte[]> callback, string url,
            bool autoFire = true,
            bool forceRetry = true)
        {
            RequestBuilder req = new RequestBuilder(url);
            req.Method(Verb.GET);
            return req.Build<byte[]>(autoFire, forceRetry, callback, (res) => { });
        }

        public RequestClass<UserModel> GetUserDataWithAuthToken(APIVersion apiVersion,GenericCallback<UserModel> callback, bool autoFire = true,
            bool forceRetry = true, string authToken = null)
        {
            RequestBuilder req = new RequestBuilder();
            req.Method(Verb.GET).AddUrlPart(apiVersion.ToString().ToLower()).AddUrlPart("auth").AddUrlPart("login").AddUrlPart("me");
            return req.Build<UserModel>(autoFire, forceRetry, callback, (res) => { UpdateUserData(res as UserModel); });
        }

        public RequestClass<UserModel> UpdateRegisteredUserData(APIVersion apiVersion,GenericCallback<UserModel> callback, bool autoFire = true,
            bool forceRetry = true)
        {
            RequestBuilder req = new RequestBuilder();
            req.Method(Verb.POST).AddUrlPart(apiVersion.ToString().ToLower()).AddUrlPart("auth").AddUrlPart("registered").AddUrlPart("update");
            return req.Build<UserModel>(autoFire, forceRetry, callback, (res) => { UpdateUserData(res as UserModel); });
        }
        
        public RequestClass<UserModel> UpdateGuestUserData(APIVersion apiVersion,GenericCallback<UserModel> callback, bool autoFire = true,
            bool forceRetry = true)
        {
            RequestBuilder req = new RequestBuilder();
            req.Method(Verb.POST).AddUrlPart(apiVersion.ToString().ToLower()).AddUrlPart("auth").AddUrlPart("guest").AddUrlPart("update");
            return req.Build<UserModel>(autoFire, forceRetry, callback, (res) => { UpdateUserData(res as UserModel); });
        }
        
      
      private void UpdateUserData(UserModel Profile)
        {
            if (!string.IsNullOrEmpty(Profile?.AuthToken))
            {
                UpdateUserToken(Profile.AuthToken);
            }

            if (Profile != null)
            {
                PlayerPrefs.SetString(m_data.PREFS_PROFILE_DATA, AES.Encrypt(JsonConvert.SerializeObject(Profile), "ud"));
            }
        }

        private void UpdateUserToken(string token)
        {
            var encripted = AES.Encrypt(token);
            PlayerPrefs.SetString(m_data.PREFS_SECRET_KEY, encripted);
            RequestHelper.Instance.Init();
        }
        
        public void LogOut()
        {
            PlayerPrefs.DeleteAll();
            RequestHelper.Instance.Init();
        }
        
        private string GetAuthToken()
        {
            string encrypted = PlayerPrefs.GetString(m_data.PREFS_SECRET_KEY);
            return AES.Decrypt(encrypted);
        }
    }

    public class ExposeToAPIEditorAttribute : Attribute
    {
    }
}