using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using FireByte;
using UnityEngine;

public class AIController : Controller<DesignData>
{
    
    [SerializeField]private LightSaber m_lightSaber;
    
    [SerializeField]private float m_aiStartRotationX;
    [SerializeField]private float m_aiEndRotationX;
    
    [SerializeField]private float m_aiStartRotationZ;
    [SerializeField]private float m_aiEndRotationZ;

    private DesignData m_designData;
    
    public override void Initialize(DesignData designData)
    {
        m_designData = designData; 
        
        m_aiStartRotationX = designData.AIStartRotationX;
        m_aiEndRotationX = designData.AIEndRotationX;
        
        m_aiStartRotationZ = designData.AIStartRotationZ;
        m_aiEndRotationZ = designData.AIEndRotationZ;   
    }
    
    public void RandomRotation()
    {
        float xRotationValue = Random.Range(m_aiStartRotationX,m_aiEndRotationX);
        float zRotationValue = Random.Range(m_aiStartRotationZ,m_aiEndRotationZ);
        
        Debug.Log(xRotationValue);
        Debug.Log(zRotationValue);
        
        m_lightSaber.RotateWithTween(xRotationValue,zRotationValue);
        DOVirtual.DelayedCall(m_designData.RotateAnimationDuration, () =>
        {
            m_lightSaber.RotateAnimateY();
        });
    }

    
}
