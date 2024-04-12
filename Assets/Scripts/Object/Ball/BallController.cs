using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private Ball m_ball;
    public Transform BallTransform { private set; get; }

    [SerializeField] private Transform m_ballStartPosition;

    public void ActiveGravity()
    {
        m_ball.Rigidbody.useGravity = true;
    }
    public void DeactiveGravity()
    {
        m_ball.Rigidbody.useGravity = false;
    }
    
    public void Initialize(Level level)
    {
        m_ball.transform.position = m_ballStartPosition.position;
        BallTransform =  m_ball.transform;
        m_ball.Initialize(level.BallMass, level.BallColor.Color);
    }

    public void AddForce(int impulseForce)
    {
        m_ball.transform.DOShakeScale(.25f, .3f, 10, 45);
        m_ball.Rigidbody.AddForce(Vector3.up * impulseForce, ForceMode.Impulse);
    }
}