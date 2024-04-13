using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManger : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_playerAddCubeParticle;
    [SerializeField] private ParticleSystem m_playerRemoveCubeParticle;
    [SerializeField] private ParticleSystem m_hitToFinishPlatformParticle;

    public void ShowAddCubeParticle(CollectableCube lastCollectableCube)
    {
        m_playerAddCubeParticle.transform.position = lastCollectableCube.transform.position;
        m_playerAddCubeParticle.Play();
    }
    
    public void ShowRemoveCubeParticle(PlayerCube hittedCube,ObstacleCube obstacleCube)
    {
        m_playerRemoveCubeParticle.transform.position = new Vector3(hittedCube.transform.position.x + hittedCube.Size.x,hittedCube.transform.position.y,hittedCube.transform.position.z);
        m_playerRemoveCubeParticle.Play();
    }

    public void ShowHitToFinishPlatformParticle(PlayerCube hittedCube)
    {
        m_hitToFinishPlatformParticle.transform.position = new Vector3(hittedCube.transform.position.x + hittedCube.Size.x,hittedCube.transform.position.y,hittedCube.transform.position.z);
        m_hitToFinishPlatformParticle.Play();
    }
}
