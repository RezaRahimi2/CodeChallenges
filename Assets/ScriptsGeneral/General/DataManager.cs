using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Immersed.General
{
    public class DataManager : Singleton<DataManager>
    {
        [SerializeField] private DataScriptableObject data;
        public static DataScriptableObject Data => Instance.data;
    }
}