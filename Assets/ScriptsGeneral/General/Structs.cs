using System;
using UnityEngine;

namespace Immersed.General
{
    [Serializable]
    public struct RequestNameWithApiVersion
    {
        [SerializeField] public RequestName RequestName;
        [SerializeField] public APIVersion APIVersion;

        public RequestNameWithApiVersion(RequestName requestName, APIVersion apiVersion)
        {
            RequestName = requestName;
            APIVersion = apiVersion;
        }
    }
}