using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CubeBase:MonoBehaviour
{
    [SerializeField]protected Material m_material;
}

public abstract class Cube<T> : CubeBase
{
    [SerializeField]protected T m_manager;
    
    public void Initialize(T t,Color cubeColor)
    {
        m_manager = t;
        m_material.color = cubeColor;
    }
}
