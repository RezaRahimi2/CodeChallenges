using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
    //parent of level game object
    [SerializeField] private Transform m_levelParent;
    //stored the current level game object
    [SerializeField] private GameObject m_currentLevelGameObject;
    [SerializeField] private Level m_currentLevel;
    [SerializeField] private Level m_nextLevel;
    //stored the number of jump platform in current level
    [SerializeField] private int m_platformCount;
    public Level CurrentLevel => m_currentLevel;
    public Level NextLevel => m_nextLevel;
    public int PlatformCount => m_platformCount;

    private int m_levelsNum;
    
    public void Initialize(int levelsNum,GameObject currentLevel, GameObject nextLevel)
    {
        m_levelsNum = levelsNum;
        
        if(m_currentLevelGameObject != null)
            Destroy(m_currentLevelGameObject);
        
        LoadLevel(currentLevel);
        m_platformCount = m_currentLevel.Platforms.Where(x => !x.HighJumpPlatform).ToList().Count;
        m_nextLevel = nextLevel.GetComponent<Level>();
    }

    public void LoadLevel(GameObject levelPrefab)
    {
        m_currentLevelGameObject = Instantiate(levelPrefab);
        m_currentLevel = m_currentLevelGameObject.GetComponent<Level>();
        m_currentLevel.transform.SetParent(m_levelParent);
    }

    //used for save index of passed level by player
    public void SaveLevel()
    {
        if (m_currentLevel.LevelNum <= m_levelsNum - 1)
        {
            PlayerPrefs.SetInt("LastLevel",m_currentLevel.LevelNum++);
        }
        else
        {
            PlayerPrefs.SetInt("LastLevel",0);
        }
    }
}