using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//the object of little wheel collider indicator hit detection
public class ObstacleObject : MonoBehaviour
{
    [SerializeField] private short m_number;
    public Action<ObstacleObject> OnCollisionEnterEvent; 
        
    public Transform GiftTextTransform
    {
        get => mGiftItemView.transform;
    }
    
    public short GiftNumber
    {
        get => m_number;
    }

    [SerializeField] private LuckyWheelGiftItemView mGiftItemView;

    public void OnCollisionEnter2D(Collision2D other)
    {
        OnCollisionEnterEvent.Invoke(this);
    }

    public void Initialize()
    {
        mGiftItemView.Initialize(out m_number);
    }

    public void ShowSelectionAnimation(bool isLastHit = false)
    {
        mGiftItemView.ShowAnimation(isLastHit);
    }
}