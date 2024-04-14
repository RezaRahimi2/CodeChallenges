using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : Manager
{
    [SerializeField] private ParticleSystem m_hitParticle;

    public override void Initialize()
    {
        m_hitParticle.Stop();
    }
    
    public void StartHitParticle(Vector3 hitPosition)
    {
        m_hitParticle.transform.position = hitPosition;
        m_hitParticle.Play();
    }
}
