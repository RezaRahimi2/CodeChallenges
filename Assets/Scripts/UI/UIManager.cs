using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


// Managing all In-game UI
public class UIManager : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI m_scoreText;
    [SerializeField]private CanvasGroup m_foulPlayPanelCanvasGroup;
    [SerializeField]private Button m_restartBtn;
    [SerializeField]private CanvasGroup m_finishLevelPanelCanvasGroup;

    public void Initialize(Action resetGameAction)
    {
        m_restartBtn.onClick.AddListener(()=>resetGameAction());
    }
    
    public void OnScoreChange(int newScore)
    {
        m_scoreText.text = newScore.ToString();
    }

    public void OnFoulPlay()
    {
        m_foulPlayPanelCanvasGroup.gameObject.SetActive(true);
        m_foulPlayPanelCanvasGroup.DOFade(1, 1);
    }

    public void OnFinishLevel()
    {
        m_finishLevelPanelCanvasGroup.gameObject.SetActive(true);
        m_finishLevelPanelCanvasGroup.DOFade(1, 1);
    }

    public void OnRestartGame(Action afterHideCallBack)
    {
        m_foulPlayPanelCanvasGroup.DOFade(0, .5f).OnComplete(() =>
        {
            m_foulPlayPanelCanvasGroup.gameObject.SetActive(false);
            afterHideCallBack.Invoke();
        });
    }
}
