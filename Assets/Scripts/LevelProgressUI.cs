using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_currentLevelTextMesh;
    [SerializeField] private TextMeshProUGUI m_nextLevelTextMesh;
    [SerializeField] private Slider levelProgressSlider;
    
    public void Initialize(int jumpPlatformsNum,int currentLevelNum, int nextLevelNum)
    {
        levelProgressSlider.maxValue = jumpPlatformsNum;
        levelProgressSlider.value = 0;
        m_currentLevelTextMesh.text = currentLevelNum.ToString();
        m_nextLevelTextMesh.text = nextLevelNum.ToString();
    }

    public void SetLevelProgression(int hittedJumpPlatformNum)
    {
        levelProgressSlider.value = hittedJumpPlatformNum;
    }
}
