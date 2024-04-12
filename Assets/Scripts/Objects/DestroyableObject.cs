using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    [SerializeField] private List<Rigidbody> m_rigidbodies;

    public void Initialize()
    {
        m_rigidbodies = GetComponentsInChildren<Rigidbody>().ToList();
    }

    public void ActiveRigidBodies()
    {
        gameObject.SetActive(true);
        m_rigidbodies.ForEach(x =>
        {
            x.useGravity = true;
            x.AddForce(new Vector3(Random.Range(0.1f, 1f), Random.Range(0.1f, 1f), Random.Range(0.1f, 1f)),
                ForceMode.Impulse);
        });
    }
}