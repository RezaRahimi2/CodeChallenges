using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Immersed.General
{
    public interface IRequestNameWithApiVersion
    {
        [field: SerializeField] public RequestNameWithApiVersion RequestNameWithApiVersion { get; set; }
    }
}