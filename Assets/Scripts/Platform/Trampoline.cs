using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    //when player hit reach to this value platform is destroy 
    private int m_destroyHitCount;
    //count the hit by player to platform
    [SerializeField] private int destroyCounter;
    [SerializeField] private JumpPlatform m_jumpPlatform;
    [SerializeField] private bool m_isDestroy;
    
    public void Initialize(int destroyHitCount,JumpPlatform jumpPlatform)
    {
        m_destroyHitCount = destroyHitCount;
        m_jumpPlatform = jumpPlatform;
        destroyCounter = 0;
    }

    public bool ShowDestroyable()
    {
        destroyCounter++;
        
        if(destroyCounter == m_destroyHitCount - 1)
            m_jumpPlatform.ShowDestroyableObject();
        // after reach to destroy counter execute the destruction method after a delay
        if (destroyCounter == m_destroyHitCount && !m_isDestroy)
        {
            m_isDestroy = true;
            DOVirtual.DelayedCall(.15f, () =>
            {
                m_jumpPlatform.ShowDestruction();
            });
        }

        return m_isDestroy;
    }
}
