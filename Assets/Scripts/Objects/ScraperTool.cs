using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class ScraperTool : MonoBehaviour
{
    [SerializeField] private Spiral _spiral;
    [SerializeField] private GameObject spiralGameObject;
    [SerializeField] private GameObject _spiralPrefab;
    [SerializeField] private BoxCollider m_collider;
    [SerializeField] private GameObject m_mainBlade;
    [SerializeField] private DestroyableObject m_destroyableBlade;
    
    private float m_startTime;

    public void Initialize()
    {
        m_destroyableBlade.Initialize();
    }
    
    public void StartScraping(SurfaceData surfaceData)
    {
        if (_spiral == null)
        {
            m_startTime = Time.time;
            spiralGameObject = Instantiate(_spiralPrefab);
            _spiral = spiralGameObject.GetComponentInChildren<Spiral>();
            _spiral.Initialize(surfaceData);
        }

        _spiral.length += .1f;
        _spiral.Refresh();
        spiralGameObject.transform.position = transform.position;
    }

    public void StopScraping()
    {
        m_collider.enabled = true;
        _spiral?.StopScraping(Time.time - m_startTime);
        _spiral = null;
    }


    public void ShowDestroyable()
    {
        m_mainBlade.SetActive(false);
        m_destroyableBlade.ActiveRigidBodies();
        
            
    }
    
}
