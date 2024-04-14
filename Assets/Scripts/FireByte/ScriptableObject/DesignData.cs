using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FireByte
{
    [CreateAssetMenu(fileName = "DesignData", menuName = "DesignData", order = 1)]
    public class DesignData : ScriptableObject
    {
        [Header("AI")] 
        public bool AIEnabled;
        [Range(0, 360)] 
        public float AIStartRotationX;
        [Range(0,360)]
        public float AIEndRotationX;
        [Range(0,360)]
        public float AIStartRotationZ;
        [Range(0,360)]
        public float AIEndRotationZ;
        
        [Header("Light Saber Animation")][Range(0,360)]
        public float GoBackAnimationAngle;
        [Range(0,360)]
        public float RotationAnimationAngle;
        
        [Header("Animation Duration")]
        [Range(.1f,10)]
        public float BeforeAnimationDuration;
        [Range(.1f,10)]
        public float AnimationDelayTime;
        [Range(.1f,10f)]
        public float BeforeStartAnimationDuration;
        [Range(.1f,10f)]
        public float RotateAnimationDuration;
        
        [Header("UI")]
        [Range(.1f, 2)] 
        public float FadeDuration;
    }
}
