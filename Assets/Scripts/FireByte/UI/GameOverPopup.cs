using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPopup : UIPopUp
{
    [SerializeField] private CanvasGroup m_canvasGroup;
    [SerializeField] private Text m_gameOverText;
    [SerializeField] private float m_fadeDuration;

    public override void Initialize(float fadeDuration)
    {
        m_fadeDuration = fadeDuration;
        Hide();
    }

    public override void Show()
    {
        gameObject.SetActive(true);
        m_canvasGroup.DOFade(1, m_fadeDuration);
    }

    public override void Hide()
    {
        m_canvasGroup.DOFade(m_fadeDuration, .5f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}