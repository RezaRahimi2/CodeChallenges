using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private Transform m_scrapperStartPosition;
    public Transform ScrapperStartPosition => m_scrapperStartPosition;

    [SerializeField] private Transform m_levelHideTransformUnderWater;
    [SerializeField] private LevelPrizeSelector m_levelPrizeSelector;
    
    private LevelManager m_levelManager;
    
    public void Initialize(LevelManager levelManager)
    {
        m_levelManager = levelManager;
        transform.position = m_levelHideTransformUnderWater.position;
        m_levelPrizeSelector = FindObjectOfType<LevelPrizeSelector>();
        m_levelPrizeSelector.Initialize(this);
    }

    public void Show()
    {
        transform.DOMoveY(0, 3).OnComplete(() =>
        {
            GameManager.Instance.LevelIsReady();
        });
    }

    public void Hide()
    {
        transform.DOMoveY(m_levelHideTransformUnderWater.position.y, 3);
    }

    public void LevelIsFinished()
    {
        m_levelManager.LevelIsFinished();
    }
}
