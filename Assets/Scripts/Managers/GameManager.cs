using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<GameManager>();

            return instance;
        }
    }

    public static bool GameStarted;
    
    [SerializeField] private Events m_events;
    
    [SerializeField] private CutManager m_cutManager;
    [SerializeField] private MovementController m_movementController;
    [SerializeField] private CoinManager m_coinManager;
    [SerializeField] private PrizeManager m_prizeManager;
    [SerializeField] private LevelManager m_levelManager;
    [SerializeField] private ObstacleManager m_obstacleManager;
    
    [SerializeField] private GameManagerData m_gameManagerData;
        
    [SerializeField] private bool hitToScraperableObject;
    [SerializeField] private SurfaceData m_surfaceData;
    
    private void Start()
    {
        Initialize(false);
    }


    public void Initialize(bool useTween)
    {
        m_events = new Events();

        m_levelManager.Initialize();
        m_coinManager.Initialize();
        m_cutManager.Initialize(m_gameManagerData.MoveSpeed);
        m_movementController.Initialize(m_gameManagerData.MoveSpeed,m_levelManager.ScrapperStartTransform,useTween);
        m_prizeManager.Initialize();
        m_obstacleManager.Initialize();

        m_events.onStartScrapEvent = null;
        m_events.onStartScrapEvent += m_movementController.StartScraping;
        
        m_events.onStopScrapEvent = null;
        m_events.onStopScrapEvent += m_movementController.StopScraping;
        m_events.onStopScrapEvent += ()=> DOVirtual.DelayedCall(.5f,()=>m_cutManager.StopScraping()) ;

        GameStarted = true;
    }

    public void HitToScraperableObject(SurfaceData surfaceData)
    {
        m_surfaceData = surfaceData;
        hitToScraperableObject = true;
    }
    
    public void StartScraping()
    {
            m_events.onStartScrapEvent?.Invoke();
    }

    public void GenerateSpiral()
    {
        if (hitToScraperableObject)
            m_movementController.GenerateSpiral(m_surfaceData);
    }
    
    public void StopScraping()
    {
        m_events.onStopScrapEvent?.Invoke();
        if (hitToScraperableObject)
        {
            m_movementController.StopGeneratingSpiral();
            hitToScraperableObject = false;
        }
    }

    public void LevelIsReady()
    {
        m_obstacleManager.EnableObstaclesGravity();
    }
    
    public void LevelIsFinished()
    {
        GameStarted = false;

        if (m_levelManager.HaveNextLevel)
        {
            DOVirtual.DelayedCall(4, () =>
            {
                m_levelManager.HideCurrentLevel();
                Initialize(true);
            });
            
        }
    }

    public void HitToObstacle()
    {
        GameStarted = false;
        StopScraping();
        m_movementController.HitToObstacle();
    }
}
