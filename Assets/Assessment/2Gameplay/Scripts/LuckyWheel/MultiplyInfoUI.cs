using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultiplyInfoUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField m_playerBalanceText;
    [SerializeField] private Image m_playerBalanceImage;
    [SerializeField] private TMP_InputField m_hittedNumberText;
    [SerializeField] private Image m_hittedNumberImage;
    [SerializeField] private TMP_InputField m_resultText;
    [SerializeField] private Image m_resultImage;

    public void SetPlayerBalance(long playerBalanceValue, float delay)
    {
        long startValue = 0;
        DOTween.To(() => startValue, x => m_playerBalanceText.text = x.ToString(), playerBalanceValue,
            2f).SetDelay(delay);

        m_playerBalanceImage.transform.DOPunchScale(new Vector3(.1f, .1f, .1f), .2f).SetLoops(10, LoopType.Yoyo)
            .SetDelay(delay);
    }

    public void SetHittedNumber(Int16 hittedNumberValue, float delay, Action afterAnimationFinishedCallback)
    {
        Int16 startValue = 0;
        DOTween.To(() => startValue, x => m_hittedNumberText.text = x.ToString(), hittedNumberValue,
            .5f).SetDelay(delay);

        m_hittedNumberImage.transform.DOPunchScale(new Vector3(.1f, .1f, .1f), .1f).SetLoops(5, LoopType.Yoyo)
            .SetDelay(delay)
            .OnComplete(() => afterAnimationFinishedCallback?.Invoke());
    }

    public void SetResultNumber(long resultValue, float delay)
    {
        Int16 startValue = 0;
        DOTween.To(() => startValue, x => m_resultText.text = x.ToString(), resultValue,
            3).SetDelay(delay);

        m_resultImage.transform.DOPunchScale(new Vector3(.2f, .2f, .2f), .3f).SetLoops(10, LoopType.Yoyo)
            .SetDelay(delay);
    }
}