using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private CoinManager m_manager;
    [SerializeField] private BoxCollider m_collider;
    public Tween RotateTween;
    public void Initialize(Tween rotateTween,CoinManager coinManager)
    {
        RotateTween = rotateTween;
        m_manager = coinManager;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        m_manager.CollectCoin(this);
    }
}
