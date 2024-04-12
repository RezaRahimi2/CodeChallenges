using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

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

    [SerializeField] private MainObject m_mainObject;

    [SerializeField] private BallController m_ballController;
    [SerializeField] private CameraController m_cameraController;
    [SerializeField] private LevelManager m_levelManager;
    [SerializeField] private InputController m_inputController;
    [SerializeField] private UIManager m_uiManager;

    public List<Object> passedPlatform;

    private int score;
    public int Score
    {
        set => m_uiManager.OnScoreChange(score = value);
        get => score;
    }

    //Initialize all manager and controllers of game and levels platforms depend the selected level, each platform generated randomly 
    public void Initialize(int levelIndex)
    {
        Level level = m_levelManager.Levels[levelIndex];

        m_cameraController.SetBackgroundColor(level.BackgroundColor.Color);
        m_mainObject.SetColor(level.MainBarColor.Color);

        m_ballController.Initialize(level);
        m_cameraController.Initialize(m_ballController.BallTransform);
        m_inputController.Initialize(m_mainObject);
        m_uiManager.Initialize(ResetGame);

        m_cameraController.gameObject.SetActive(true);
        m_inputController.gameObject.SetActive(true);

        passedPlatform = new List<Object>();

        ResetPlatforms();

        for (int i = 0; i < m_mainObject.Platforms.Count - 1; i++)
        {
            List<InteractiveObject> platformObjects = m_mainObject.Platforms[i].Objects.ToList();

            //Deactive platforms if the active flag is false in unity inspector  
            if (!level.PlatformData[i].IsActive)
            {
                m_mainObject.Platforms[i].gameObject.SetActive(false);
                platformObjects.Remove(platformObjects[i]);
                continue;
            }

            //Select Neutral platform pieces with NeutralObjectsNumber number randomly
            for (int j = 0; j < level.PlatformData[i].NeutralObjectsNumber; j++)
            {
                int rnd = Random.Range(0, platformObjects.Count - 1);

                platformObjects[rnd].Initialize(InteractiveObjectType.Neutral, level.NeutralPlatformColor.Color,
                    onHitAction: (hittedObj) => AddForce());

                platformObjects.Remove(platformObjects[rnd]);
            }

            //Select Gap platform pieces with GapObjectsNumber number randomly
            for (int j = 0; j < level.PlatformData[i].GapObjectsNumber; j++)
            {
                int rnd = Random.Range(0, platformObjects.Count - 1);

                platformObjects[rnd].Initialize(InteractiveObjectType.Gap, Color.white,
                    onPassAction: (passedObj) =>
                    {
                        if (!passedPlatform.Contains(passedObj))
                        {
                            Pass();
                            passedPlatform.Add(passedObj);
                        }
                    });

                platformObjects.Remove(platformObjects[rnd]);
            }

            //Select Foul platform pieces with FoulObjectNumbers number randomly
            for (int j = 0; j < level.PlatformData[i].FoulObjectNumbers; j++)
            {
                int rnd = Random.Range(0, platformObjects.Count - 1);
                platformObjects[rnd].Initialize(InteractiveObjectType.Foul, level.FoulPlatformColor.Color,
                    onFoulAction: (foulObj) => { Foul(); });
                platformObjects.Remove(platformObjects[rnd]);
            }
        }

        //Last platform in each level is goal platform
        m_mainObject.Platforms[m_mainObject.Platforms.Count - 1].Objects.ForEach(x =>
        {
            x.Initialize(InteractiveObjectType.Goal, level.GoalPlatformColor.Color,
                onGoalAction: (goalObj) => EndGame());
        });
    }

    public void StartGame()
    {
        m_ballController.ActiveGravity();
    }

    public void EndGame()
    {
        m_ballController.DeactiveGravity();
        m_cameraController.gameObject.SetActive(false);
        m_uiManager.OnFinishLevel();
    }

    //Reset platform to initialize state
    public void ResetPlatforms()
    {
        for (int i = 0; i < m_mainObject.Platforms.Count; i++)
        {
            m_mainObject.Platforms[i].gameObject.SetActive(true);
            m_mainObject.Platforms[i].Objects
                .ForEach(x => { x.Initialize(InteractiveObjectType.Neutral, Color.white); });
        }
    }

    public void ResetGame()
    {
        Score = 0;
        m_uiManager.OnRestartGame(()=>Initialize(PlayerPrefs.GetInt("LastPlayedLevel", 0)));
    }

    //Add force to ball when hit to a Neutral piece for bouncing movement
    public void AddForce()
    {
        m_ballController.AddForce(5);
    }

    //Called when ball passes through a gap piece 
    public void Pass()
    {
        ++Score;
    }

    //Called when ball hit to a foul piece  
    public void Foul()
    {
        m_inputController.gameObject.SetActive(false);
        m_ballController.gameObject.SetActive(false);
        m_uiManager.OnFoulPlay();
    }
}