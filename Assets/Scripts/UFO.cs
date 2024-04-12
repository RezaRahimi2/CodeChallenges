using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UFO : MonoBehaviour
{
    private int m_destroyHitCount;
    [SerializeField] private List<ParticleSystem> m_fireParticles;
    [SerializeField] private ParticleSystem m_explodeParticle;
    [SerializeField] private int fireShowParticleCounter;
    [SerializeField] private JumpPlatform m_jumpPlatform;
    [SerializeField] private bool m_isDestroy;
    public void Initialize(int destroyHitCount,JumpPlatform jumpPlatform)
    {
        m_destroyHitCount = destroyHitCount;
        m_jumpPlatform = jumpPlatform;
        fireShowParticleCounter = 0;
        m_fireParticles.ForEach(x=>x.Stop());
        m_explodeParticle.Stop();
    }
    
    public void ShowFire()
    {
        if (fireShowParticleCounter < m_fireParticles.Count)
        {
            m_fireParticles[fireShowParticleCounter].Play();
        }
        
        fireShowParticleCounter++;

        if (fireShowParticleCounter == m_destroyHitCount && !m_isDestroy)
        {
            m_isDestroy = true;
            m_explodeParticle.gameObject.SetActive(true);
            DOVirtual.DelayedCall(.15f, () =>
            {
                m_jumpPlatform.ShowDestruction();
            });
            
        }
    }

}
