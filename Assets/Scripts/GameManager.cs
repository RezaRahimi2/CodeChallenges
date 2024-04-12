using DG.Tweening;
using UnityEngine;

//The managing game class a gate way for connecting classes 
public class GameManager : MSingleton<GameManager>
{
    #region Static Fields
    //a Static flag used for Game started or not 
    public static bool GameStarted;

    #endregion

    //Reference of scriptable objects data
    #region Data

    [SerializeField] private GameManagerData m_gameManagerData;
    [SerializeField] private LevelsData m_levelsData;

    #endregion

    //Reference of managers and controllers
    #region Managers and Controllers

    [SerializeField] private LevelManager m_levelManager;
    [SerializeField] private PlatformManager mPlatformManager;
    [SerializeField] private InputController m_inputController;
    [SerializeField] private PlayerController m_playerController;
    [SerializeField] private UIManager m_uiManager;
    [SerializeField] private ParticleManager m_particleManager;
    [SerializeField] private CameraManager m_cameraManager;
    [SerializeField] private AudioManager m_audioManager;

    #endregion
    
    //instance of events
    #region Events
    //an event used for when game is started
    private StartGameEvent m_startGameEvent;
    //an event used for when player hit to a platform
    private HitToPlatformEvent m_HitToPlatformEventEvent;
    //an event used for when player finished a level
    private LevelIsFinishedEvent m_levelIsFinishedEvent;
    //an event used for when player is game over
    private GameOverEvent m_gameOverEvent;

    #endregion

    //initialize the game manager, we can use the initialize method in loading page for preparing a game
    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        //load the index of last played level
        int lastLevelNum = PlayerPrefs.GetInt("LastLevel");

        //initialize the level manager by passing the level number, current and next level
        m_levelManager.Initialize(m_levelsData.LevelsPrefab.Count, m_levelsData.LevelsPrefab[lastLevelNum],
            m_levelsData.LevelsPrefab[
                (lastLevelNum == m_levelsData.LevelsPrefab.Count - 1) ? lastLevelNum : lastLevelNum + 1]);
        //initialize the platform manager by passing the platform list in the level
        mPlatformManager.Initialize(m_levelManager.CurrentLevel.Platforms);
        //initialize the player controller by passing the player start transform and platform layer mask
        m_playerController.Initialize(m_levelManager.CurrentLevel.PlayerStartTransform,
            m_gameManagerData.PlatformLayerMask);
        //initialize the input controller by passing the move speed and rotate speed from game manager data 
        m_inputController.Initialize(m_gameManagerData.MoveSpeed, m_gameManagerData.RotateSpeed);
        //initialize the ui manager by passing number of platforms, current level number and next level number  
        m_uiManager.Initialize(m_levelManager.PlatformCount, m_levelManager.CurrentLevel.LevelNum,
            m_levelManager.NextLevel.LevelNum);
        //initialize the particle manager by passing the victory particles from last platform
        m_particleManager.Initialize(mPlatformManager.FinalPlatform.VictoryParticles);
        m_audioManager.Initialize();
        m_cameraManager.Initialize();

        m_startGameEvent = null;
        m_HitToPlatformEventEvent = null;
        m_levelIsFinishedEvent = null;
        m_gameOverEvent = null;
        
        m_startGameEvent += () => GameStarted = true;
        m_startGameEvent += m_uiManager.GameStarted;

        m_HitToPlatformEventEvent += m_audioManager.HitToPlatformSound;
        m_HitToPlatformEventEvent += m_uiManager.HitToJumpPlatform;

        m_levelIsFinishedEvent += () =>
        {
            GameStarted = false;
            m_inputController.enabled = false;
            DOTween.KillAll();
        };
        m_levelIsFinishedEvent += m_levelManager.SaveLevel;
        m_levelIsFinishedEvent += m_playerController.LevelIsFinished;
        m_levelIsFinishedEvent += m_particleManager.PlayFinishParticles;
        m_levelIsFinishedEvent += () => { DOVirtual.DelayedCall(4, Initialize); };

        m_gameOverEvent += () =>
        {
            m_inputController.enabled = false;
            GameStarted = false;
        };
        m_gameOverEvent += ()=> DOTween.KillAll();
        m_gameOverEvent += Initialize;

        m_inputController.enabled = true;
    }

    public void StartGame()
    {
        m_startGameEvent?.Invoke();
    }

    public void HitToPlatform(int platformNum, Transform player, float force)
    {
        if (player.CompareTag("Player"))
            m_playerController.AddForceToPlayer(force);

        if (GameStarted)
            m_HitToPlatformEventEvent?.Invoke(platformNum);
    }

    public void GetInput(float xRotateXalue, float zMovementValue)
    {
        m_playerController.MovePlayer(zMovementValue);
        m_playerController.RotatePlayer(xRotateXalue);
    }

    public void LevelIsFinished(int lastPlatformNum)
    {
        m_HitToPlatformEventEvent -= m_audioManager.HitToPlatformSound;
        m_HitToPlatformEventEvent?.Invoke(lastPlatformNum);
        m_levelIsFinishedEvent?.Invoke();
        
    }

    public void GameOver()
    {
        m_gameOverEvent?.Invoke();
    }
}