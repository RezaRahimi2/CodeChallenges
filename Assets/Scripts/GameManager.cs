using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    //Singleton pattern of game manager
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<GameManager>();

            return instance;
        }
    }
    private static GameManager instance;
    #endregion
    

    public static bool GameStarted;
    private Events m_events;

    [SerializeField] private GameManagerData m_gameManagerData;

    [SerializeField] private InputController m_inputController;
    [SerializeField] private PlayerController m_playerController;
    [SerializeField] private MovementController m_movementController;
    [SerializeField] private CameraMovementController m_cameraMovementController;

    [SerializeField] private PlatformManager m_platformManager;
    [SerializeField] private ObstacleManager m_obstacleManager;
    [SerializeField] private CollectManager m_collectManager;
    [SerializeField] private ParticleManger m_particleManger;
    [SerializeField] private AudioManager m_audioManager;


    private bool m_hitToObstacleCoolDown;

    // we can use Initialize method after loading screen 
    public void Awake()
    {
        Initialize();
    }

    // Initialize all managers and controller
    public void Initialize()
    {
        m_platformManager.Initialize();
        m_obstacleManager.Initialize(m_gameManagerData.ObstacleCubeColorSet, m_gameManagerData.FinishPlatformColorSet);
        Color trailColor = m_collectManager.Initialize(m_gameManagerData.CollectableCubeColorSet);
        m_playerController.Initialize(trailColor);

        m_movementController.Initialize(m_playerController.PlayerTransform);
        m_inputController.Initialize(m_playerController.Player, m_platformManager.HittedPlatformBoxCollider);
        m_cameraMovementController.Initialize(m_playerController.PlayerTransform);
        m_audioManager.Initialize(m_playerController.Player.AudioSource);

        
        m_events = new Events();
        //add particle show and play sound and add hitted cube add to player  to AddCubeEvent 
        m_events.OnAddCubeEvent += m_playerController.AddCubeToPlayer;
        m_events.OnAddCubeEvent += m_particleManger.ShowAddCubeParticle;
        m_events.OnAddCubeEvent += (collectableCube) => m_audioManager.PlayAddCube();
    
        //play sound and speed up for a while to OnHitToObstacleRunOneTime run in each collision one time with cool down method 
        m_events.OnHitToObstacleRunOneTime += m_audioManager.PlaySlideSound;
        m_events.OnHitToObstacleRunOneTime += () => m_movementController.SpeedUp();

        //add particle show and play sound and remove hitted cube from player cubes to OnHitToFinishPlatform 
        m_events.OnHitToFinishPlatform += m_playerController.RemoveCubeFromPlayer;
        m_events.OnHitToFinishPlatform += m_particleManger.ShowHitToFinishPlatformParticle;
        m_events.OnHitToFinishPlatform += (collectableCube) => m_audioManager.PlaySlideSound();

        m_events.OnHitToObstacleCubeEvent += (cube, obstacleCube) => m_playerController.RemoveCubeFromPlayer(cube);
        m_events.OnHitToObstacleCubeEvent += m_particleManger.ShowRemoveCubeParticle;

        m_events.OnGameIsOverEvent += winner =>
        {
            GameStarted = false;
            m_movementController.gameObject.SetActive(false);
            m_inputController.gameObject.SetActive(false);
        };
        m_events.OnGameIsOverEvent += m_playerController.GameIsOver;
    }

    public void StartGame()
    {
        GameStarted = true;
    }

    //Pass newpos vector3 to player controller for move player horizontally
    public void GetInput(Vector3 newPos)
    {
        m_playerController.SetPlayerPosition(newPos);
    }

    //Called when player cube hit to obstacle cube, in each hit vertically cubes run onece 
    public void HitToObstacle(PlayerCube playerCube, int obstaclesCubeCount, ObstacleCube obstacleCube)
    {
        if (!m_hitToObstacleCoolDown)
        {
            if (obstaclesCubeCount >= m_playerController.Player.PlayerCubeCount)
            {
                m_events.OnGameIsOverEvent?.Invoke(false);
            }
            else
            {
                m_hitToObstacleCoolDown = true;
                HitCoolDown();
                m_events.OnHitToObstacleRunOneTime?.Invoke();
            }
        }

        m_events.OnHitToObstacleCubeEvent?.Invoke(playerCube, obstacleCube);
    }
    //Use cool down method for take a break between obstacles hit 
    private async UniTask HitCoolDown()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1));
        m_hitToObstacleCoolDown = false;
    }
    
    public void HitToFinishPlatform(PlayerCube playerCube)
    {
        if(m_playerController.Player.PlayerCubeCount != 1)
            m_events.OnHitToFinishPlatform?.Invoke(playerCube);
        else
            m_events.OnGameIsOverEvent?.Invoke(true);
    }

   //Called when player cube hit to collectable cube/s
    public void HitToCollectableCube(CollectableCube collectableCube)
    {
        m_events.OnAddCubeEvent?.Invoke(collectableCube);
    }
}