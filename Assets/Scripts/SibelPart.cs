using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SibelPart : MonoBehaviour
{
   private JumpPlatform _mJumpPlatform;
   public float StartYValue;
   public Vector3 StartScaleSize; 

   
   public void Initialize(JumpPlatform jumpPlatform)
   {
      _mJumpPlatform = jumpPlatform;
      StartYValue = transform.position.y;
      StartScaleSize = transform.localScale;
   }

   private void OnCollisionEnter(Collision other)
   {
      if (other.transform.CompareTag("Player"))
      {
         _mJumpPlatform.HitToPlatform(other.transform);
      }

   }

}
