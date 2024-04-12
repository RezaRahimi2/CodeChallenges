using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    [SerializeField] private List<Rigidbody> m_destroyableRigidbodies;
    [SerializeField] private MeshCollider m_mainMeshCollider;
    public void Initialize()
    {
        m_destroyableRigidbodies.AddRange(GetComponentsInChildren<Rigidbody>().ToList());
    }
    
    public void EnableRigidBody()
    {
        gameObject.SetActive(true);
        
        if (m_mainMeshCollider != null)
            Destroy(m_mainMeshCollider.gameObject);
        
        m_destroyableRigidbodies.ForEach(x =>
        {
            x.useGravity = true;
            x.AddForce( new Vector3(Random.Range(0f,100f),Random.Range(0f,-100f),Random.Range(0f,100f)),ForceMode.Impulse);
        });

        
    }
}
