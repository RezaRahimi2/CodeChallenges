using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Get inputs and pass through to game manager
public class InputController : MonoBehaviour
{
    //movement speed and rotate speed
    [SerializeField] private float m_moveSpeed;
    [SerializeField] private float m_rotateSpeed;
    
    private float? lastMouseXPoint = null;
    private float? lastMouseYPoint = null;

    //Initialize method of Input controller for get the rotation speed and move speed from a game manager data serializable object
    public void Initialize(float moveSpeed, float rotationSpeed)
    {
        m_moveSpeed = moveSpeed;
        m_rotateSpeed = rotationSpeed;
        lastMouseXPoint = null;
        lastMouseYPoint = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && GameManager.GameStarted)
        {
            if (Input.GetMouseButtonDown(0))
            {
                lastMouseXPoint = Input.mousePosition.x;
                lastMouseYPoint = Input.mousePosition.y;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                lastMouseXPoint = null;
                lastMouseYPoint = null;
            }

            if (lastMouseXPoint != null && lastMouseYPoint != null)
            {
                float differenceX = Input.mousePosition.x - lastMouseXPoint.Value;
                float differenceY = Input.mousePosition.y - lastMouseYPoint.Value;
                //transform.position = new Vector3(transform.position.x + (difference / 188) * Time.deltaTime, transform.position.y, transform.position.z);
                GameManager.Instance.GetInput(differenceX * Time.fixedDeltaTime * m_rotateSpeed,
                    Mathf.Abs((differenceY / 188) * Time.fixedDeltaTime * m_moveSpeed));
                lastMouseXPoint = Input.mousePosition.x;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (!GameManager.GameStarted && Input.GetMouseButton(0))
            GameManager.Instance.StartGame();
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}