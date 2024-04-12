using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScraperableWood : MonoBehaviour
{
    [SerializeField]private CutManager _manager;
    public BoxCollider Collider;
    [SerializeField] private float m_reduceScaleSpeed;
    [SerializeField] private ReduceSize m_reduceSize;
    [SerializeField] private SurfaceData m_data;
    
    public void Initialize(float reduceScaleSpeed,CutManager cutManager)
    {
        _manager = cutManager;
    }
    
    private void OnCollisionEnter(Collision other)
    {

        if (other.transform.CompareTag("Scraper"))
        {
            //Collider.enabled = false;
            
            _manager.Cut(this,m_data,transform.parent,other.contacts[0].point,
                other.contacts[other.contacts.Length - 1].point,
                other.contacts[other.contacts.Length - 1].normal);
           
            m_reduceSize.enabled = true;
        }
    }
    
}
