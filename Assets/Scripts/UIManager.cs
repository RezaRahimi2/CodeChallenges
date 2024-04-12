using DG.Tweening;
using TMPro;
using UnityEngine;

//used for managing UI elements
public class UIManager : MonoBehaviour
{
    //reference of Level slider bar object 
    [SerializeField] private LevelProgressUI m_levelProgressUI;
    //reference of "Perfect!" text as a hit to platform text
    [SerializeField] private TextMeshProUGUI m_hitToPlatformText;
    //reference of "tap to start" text mesh 
    [SerializeField] private TextMeshProUGUI m_startGameText;

    public void Initialize(int jumpPlatformsNum, int currentLevelNum, int nextLevelNum)
    {
        m_startGameText.DOFade(1, .5f);
        m_levelProgressUI.Initialize(jumpPlatformsNum, currentLevelNum, nextLevelNum);
        m_hitToPlatformText.alpha = 0;
    }

    public void GameStarted()
    {
        m_startGameText.DOFade(0, .5f);
    }

    //show hit to platform text with tweening
    public void HitToJumpPlatform(int jumpPlatformNum)
    {
        if (jumpPlatformNum != -1)
            m_levelProgressUI.SetLevelProgression(jumpPlatformNum);
        
        m_hitToPlatformText.alpha = 0;
        m_hitToPlatformText.transform.localScale = Vector3.one;
        m_hitToPlatformText.DOFade(1, 1f);

        m_hitToPlatformText.transform.DOScaleY(0, 1).SetDelay(1.5f);
        m_hitToPlatformText.DOFade(0, .5f).SetDelay(1.5f);
    }
}