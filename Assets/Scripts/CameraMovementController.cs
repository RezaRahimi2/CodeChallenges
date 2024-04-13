using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementController : MonoBehaviour
{
    [SerializeField] private Camera m_camera;
    [SerializeField] private Transform m_playerTransform;
    [SerializeField] private float m_offset;
    public void Initialize(Transform playerTransform)
    {
        m_playerTransform = playerTransform;
    }

    public void Update()
    {
        if (GameManager.GameStarted)
            m_camera.transform.position = new Vector3(m_playerTransform.position.x - m_offset,
                m_camera.transform.position.y,
                m_camera.transform.position.z);
    }
}