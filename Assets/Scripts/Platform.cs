using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private BoxCollider m_boxCollider;
    public BoxCollider BoxCollider => m_boxCollider;
    
    private PlatformManager m_manager;
    
    public void Initialize(PlatformManager manager)
    {
        m_manager = manager;
    }
    private void OnTriggerEnter(Collider other)
    {
        m_manager.SetPlatform(this);
    }
}
