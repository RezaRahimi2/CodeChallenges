using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private ScraperTool m_scraperTool;
    [SerializeField] private float m_moveSpeed;

    [SerializeField] private Transform scraperUpPosition;
    [SerializeField] private Transform scraperDownPosition;

    public void Initialize(float moveSpeed,Transform scrapperStartTransform,bool useTweenAnimation)
    {
        m_scraperTool.Initialize();
        m_moveSpeed = moveSpeed;
        if (useTweenAnimation)
            m_scraperTool.transform.DOMoveX(scrapperStartTransform.position.x, 1.5f);
        else
            m_scraperTool.transform.position = new Vector3(scrapperStartTransform.position.x,
                m_scraperTool.transform.position.y, m_scraperTool.transform.position.z);
    }

    public void Update()
    {
        if (GameManager.GameStarted)
        {
            m_scraperTool.transform.position += Vector3.right * m_moveSpeed * Time.deltaTime;
        }
    }

    public void GenerateSpiral(SurfaceData surfaceData)
    {
        m_scraperTool.StartScraping(surfaceData);
    }

    public void StopGeneratingSpiral()
    {
        m_scraperTool.StopScraping();
    }

    public void StartScraping()
    {
        if (DOTween.IsTweening("StartScraping"))
            DOTween.Kill("StartScraping");

        m_scraperTool.transform.DOMoveY(scraperDownPosition.position.y, .5f).SetTarget("StartScraping");
    }

    public void StopScraping()
    {
        if (DOTween.IsTweening("StartScraping"))
            DOTween.Kill("StartScraping");

        if (DOTween.IsTweening("StopScrapping"))
            DOTween.Kill("StopScrapping");

        m_scraperTool.transform.DOMoveY(scraperUpPosition.position.y, .5f).SetTarget("StopScrapping");
    }

    public void HitToObstacle()
    {
        m_scraperTool.ShowDestroyable();
    }
}
