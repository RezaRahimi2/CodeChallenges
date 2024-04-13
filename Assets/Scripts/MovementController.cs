using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private SplineTrailRenderer m_trailReference;
    [SerializeField] private Transform m_playerTransform;
    [SerializeField] private float m_moveSpeed;
    private float distance = 0;
    public void Initialize(Transform playerTransform)
    {
        m_playerTransform = playerTransform;
    }

    public void Update()
    {
        if (GameManager.GameStarted)
        {
            m_playerTransform.position = new Vector3( m_playerTransform.position.x + m_moveSpeed * Time.deltaTime,m_playerTransform.transform.position.y,m_playerTransform.position.z);
        }
    }

    public async UniTask SpeedUp()
    {
        m_moveSpeed += 6f;
        m_playerTransform.DOMoveY(m_playerTransform.position.y + .5f, .1f);
        await UniTask.Delay(TimeSpan.FromSeconds(.25f), ignoreTimeScale: false);
        m_playerTransform.DOMoveY(m_playerTransform.position.y - .5f, .1f);
        m_moveSpeed -= 6f;
    }
}
