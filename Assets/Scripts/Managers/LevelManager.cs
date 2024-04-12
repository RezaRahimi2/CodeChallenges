using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<Level> m_levelsPrefab;
    [SerializeField] private Level m_currentLevel;
    [SerializeField] private int m_levelIndex;
    public bool HaveNextLevel => m_levelIndex < m_levelsPrefab.Count - 1;

    public Transform ScrapperStartTransform => m_currentLevel.ScrapperStartPosition;

    public void Initialize()
    {
        m_levelIndex = PlayerPrefs.GetInt("LevelIndex");

        m_currentLevel = Instantiate(m_levelsPrefab[m_levelIndex]);
        m_currentLevel.Initialize(this);

        m_currentLevel.Show();
    }


    public void HideCurrentLevel()
    {
        m_currentLevel.Hide();
    }

    public void ShowNextLevel()
    {
        if (m_levelIndex < m_levelsPrefab.Count - 1)
        {
            m_currentLevel = Instantiate(m_levelsPrefab[++m_levelIndex]);

            m_currentLevel.Initialize(this);

            m_currentLevel.Show();
        }
    }

    public void LevelIsFinished()
    {
        GameManager.Instance.LevelIsFinished();
        if (HaveNextLevel)
            PlayerPrefs.SetInt("LevelIndex", ++m_levelIndex);
    }
}