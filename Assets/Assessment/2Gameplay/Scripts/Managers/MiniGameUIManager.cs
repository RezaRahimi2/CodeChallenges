using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// a manager for initialize the wheel controller
public class MiniGameUIManager : MonoBehaviour
{
    [SerializeField] private Image m_blocker;
    [SerializeField] private LuckyWheelController m_luckyWheelController;
    [SerializeField] private TextMeshProUGUI m_waitingForServerResponse;

    [SerializeField] private long m_playerBalance;
    [SerializeField] private MultiplyInfoUI mMultiplyInfoUI;
    //For storing the Waiting text tween
    private Tween waitingTextTween;
    
    
    [SerializeField] private Transform m_startTransfrom;
    [SerializeField] private Transform m_endTransfrom;
    
    //action for using after claim movement animation finished
    private Action<long> m_afterFinishMiniGameCallback = null;

    private MiniGameName m_miniGameName;

    public void ShowWaitingForServerAnimation()
    {
        m_waitingForServerResponse.alpha = 0;
        m_waitingForServerResponse.gameObject.SetActive(true);
        waitingTextTween = m_waitingForServerResponse.DOFade(1, .5f).SetLoops(-1, LoopType.Yoyo).SetTarget("Waiting");
    }

    public void HideWaitingForServerAnimation()
    {
        m_waitingForServerResponse.DOFade(0, .5f);
    }

    public void Initialize(MiniGameName miniGameName, long playerBalance,Action<long> afterMiniGameFinished)
    {
        m_playerBalance = playerBalance;
        waitingTextTween.Kill();
        HideWaitingForServerAnimation();

        m_waitingForServerResponse.DOFade(1, .5f);
        
        m_miniGameName = miniGameName;
        if (m_miniGameName == MiniGameName.LuckyWheel)
        {
            m_luckyWheelController.Initialize(
                AfterSpinFinished);
        }

        m_afterFinishMiniGameCallback = afterMiniGameFinished;
    }

    private void AfterSpinFinished(Int16 hittedNumber)
    {
        mMultiplyInfoUI.SetHittedNumber(hittedNumber, .5f, () =>
        {
            long updatedPlayerBalance = m_playerBalance * hittedNumber;
            m_afterFinishMiniGameCallback?.Invoke(updatedPlayerBalance);
            mMultiplyInfoUI.SetResultNumber(updatedPlayerBalance,.5f);
        });
    }
    
    public void MiniGameShow()
    {
        if (m_miniGameName == MiniGameName.LuckyWheel)
        {
            m_blocker.color = new Color(0, 0, 0, 0);
            m_blocker.gameObject.SetActive(true);
            m_blocker.DOFade(.5f, .5f).OnComplete(() =>
            {
                mMultiplyInfoUI.SetPlayerBalance(m_playerBalance,1);
            });
            m_luckyWheelController.Show(m_endTransfrom);
        }
    }
}