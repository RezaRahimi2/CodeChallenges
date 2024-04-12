using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Input controlling of the main object to rotate
public class InputController : MonoBehaviour
{
    private MainObject m_mainObject;
    [SerializeField]private Vector3 m_startRotation;
    [SerializeField]private Vector2 m_lastTapPos;

    public void Initialize(MainObject mainObject)
    {
        m_startRotation = Vector3.zero;
        m_lastTapPos = Vector2.zero;
        m_mainObject = mainObject;
    }
    
    void Update()
    {
        if (Input.GetMouseButton(0))
        {

            Vector2 curTapPos = Input.mousePosition;

            if (m_lastTapPos == Vector2.zero)
                m_lastTapPos = curTapPos;

            float delta = m_lastTapPos.x - curTapPos.x;
            m_lastTapPos = curTapPos;

            m_mainObject.transform.Rotate(Vector3.up * delta);
        }

        if (Input.GetMouseButtonUp(0))
        {
            m_lastTapPos = Vector2.zero;
        }
    }
}
