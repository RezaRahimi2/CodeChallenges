using System;
using UnityEngine;
using Object = System.Object;

namespace Immersed.General
{
    [Serializable]
    public class Response<T>
    {
        public bool status;
        public string message;
        public T response;
        public string responseText;
        public long code;
        public int totalpage;
        public Boolean isSuccessfull()
        {
            return status;
        }
        
        public static implicit operator byte[](Response<T> left)
        {
            if (typeof(T) == typeof(byte[]))
            {
                return (byte[])(object)left;
            }
            else
            {
                return null;
            }
        }
    }
}

