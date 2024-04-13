using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    [SerializeField] private List<Platform> m_platforms;
    [SerializeField] private Platform m_hittedPlatform;

    public BoxCollider HittedPlatformBoxCollider => m_hittedPlatform.BoxCollider;
        
    public void Initialize()
    {
        m_platforms = FindObjectsOfType<Platform>().ToList();
        
        m_platforms.ForEach(x =>
        {
            x.Initialize(this);
        });
        m_platforms = m_platforms.OrderBy(platform => (Camera.main.transform.position - platform.transform.position).sqrMagnitude).ToList();
        m_hittedPlatform = m_platforms[0];
    }
    

    public void SetPlatform(Platform platform)
    {
        m_hittedPlatform = platform;
    }
}
