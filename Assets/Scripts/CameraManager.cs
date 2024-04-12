using UnityEngine;

//for managing camera movement
public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera m_camera;
    //reference of start camera position
    [SerializeField]private Transform m_startCameraPosition;
    //reference of follow camera position
    [SerializeField]private Transform m_followPlayerCameraPosition;

    //set camera position and rotation to start position 
    public void Initialize()
    {
        m_camera.transform.position = m_startCameraPosition.position;
        m_camera.transform.rotation = m_startCameraPosition.rotation;
    }

    private void FixedUpdate()
    {
        if (GameManager.GameStarted && Vector3.Distance(m_camera.transform.position,m_followPlayerCameraPosition.position) > .01f)
        {
            m_camera.transform.position = Vector3.Lerp(m_camera.transform.position,
                m_followPlayerCameraPosition.position, 3 * Time.deltaTime);
            m_camera.transform.eulerAngles = Vector3.Lerp(m_camera.transform.eulerAngles,
                m_followPlayerCameraPosition.eulerAngles, 3 * Time.deltaTime);
        }
    }
}
