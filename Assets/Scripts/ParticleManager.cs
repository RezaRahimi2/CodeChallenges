using System.Collections.Generic;
using UnityEngine;

//for managing the particle
public class ParticleManager : MonoBehaviour
{
    //reference of final particles
    [SerializeField] private List<ParticleSystem> m_finishParticles;

    //initialize the final particles from Last Platform Object
    public void Initialize(List<ParticleSystem> particles)
    {
        m_finishParticles = particles;
    }

    public void PlayFinishParticles()
    {
        m_finishParticles.ForEach(x => { x.Play(); });
    }
}