using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCube : MonoBehaviour
{
    [SerializeField] private BoxCollider m_boxCollider;
    [SerializeField] private Rigidbody m_rigidbody;
    public Vector3 Size => m_boxCollider.size;

    public void Initialize()
    {
        m_boxCollider = GetComponent<BoxCollider>();
        m_rigidbody = GetComponent<Rigidbody>();
    }

    public void RemoveRigidBody()
    {
        Destroy(m_rigidbody);
    }
}
