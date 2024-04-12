using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//controlling the camera movement with an offset follow the ball
public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera m_mainCamera;
    private Transform m_mainCameraTransform;
    [SerializeField] private float m_offset,m_initialOffset = 2.4f;
    private Transform m_ballTramsform;

    public void Initialize(Transform ballTransform)
    {
        m_ballTramsform = ballTransform;
        m_mainCameraTransform = m_mainCamera.transform;
        m_offset = m_initialOffset;
    }

    public void SetBackgroundColor(Color color)
    {
        m_mainCamera.backgroundColor = color;
    }

    private void Update()
    {
        Vector3 curPos = m_mainCamera.transform.position;
        curPos.y = m_ballTramsform.transform.position.y + m_offset;
        m_mainCameraTransform.position = curPos;
    }
}