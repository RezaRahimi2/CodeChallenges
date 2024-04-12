using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MovingPlatfom : JumpPlatform,IMoveable
{
    [SerializeField] private Vector3 startPositionValue;
    [SerializeField] private Transform m_rightTransform;
    [SerializeField] private Transform m_leftTransform; 
     
    public void Start()
    {
        startPositionValue = transform.position;
        Move();
    }
    
    public void Move()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(m_rightTransform.position, 4));
        sequence.Append(transform.DOMove(m_leftTransform.position,8));
        sequence.SetLoops(-1, LoopType.Yoyo);
        sequence.SetEase(Ease.Linear);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position,transform.forward * 10);
        Gizmos.DrawRay(transform.position,transform.right * 10);
    }
}
