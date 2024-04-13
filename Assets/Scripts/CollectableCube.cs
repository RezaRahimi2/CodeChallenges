using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableCube : Cube<CollectManager>
{
    private bool m_collected;
    [SerializeField] private BoxCollider m_boxCollider;
    public Vector3 Size => m_boxCollider.size;

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("PlayerCube") && !m_collected)
        {
            m_collected = true;
            other.transform.localPosition = new Vector3(0,other.transform.localPosition.y,0);
            m_manager.HitToCollectableCube(this);
        }
    }
}