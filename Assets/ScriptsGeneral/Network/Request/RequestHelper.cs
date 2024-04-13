using System;
using System.Collections.Generic;
using UnityEngine;

namespace Immersed.General
{
    public class RequestHelper : Singleton<RequestHelper>
    {
        public string BaseUrl;

        public Dictionary<string, string> Headers;

        public void Init()
        {
            Headers = new Dictionary<string, string>();
            Headers.Add("Content-Type", "application/json");
            Headers.Add("OS", SystemInfo.operatingSystem);
            Headers.Add("DeviceId", SystemInfo.deviceUniqueIdentifier);
            Headers.Add("DeviceName", SystemInfo.deviceName);
            Headers.Add("Version", "sdk_1.0");
            if (PlayerPrefs.HasKey(DataManager.Data.PREFS_SECRET_KEY))
            {
                Headers.Add("SecretKey", AES.Decrypt(PlayerPrefs.GetString(DataManager.Data.PREFS_SECRET_KEY)));
            }
        }

        void Awake()
        {
            if (RequestHelper.Instance != this)
            {
                DestroyImmediate(gameObject);
                return;
            }
            else
            {
                Init();
            }
        }

        //public new void OnDestroy ()
        //{
        //	base.OnDestroy ();
        //}
    }
}