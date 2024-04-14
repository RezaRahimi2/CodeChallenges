using System.Collections;
using System.Collections.Generic;
using FireByte;
using UnityEngine;

public class LightSaberCollider : MonoBehaviour
{
   private LightSaber m_lightSaber;
   private Vector3 offset;
   [SerializeField]private Transform parentTransform;
   
   void Start()
   {
       // Get reference to the parent object's transform
       //parentTransform = transform.parent;
       // Calculate the offset between the child and the parent
       offset = transform.position - parentTransform.position;
   }

   void Update()
   {
      // Match the child's rotation to the parent's rotation
      transform.localRotation = Quaternion.identity;

      // Apply offset to maintain distance from parent
      transform.position = parentTransform.position;
   }

   public void Initialize(LightSaber lightSaber)
   {
      m_lightSaber = lightSaber;
   }
   
   private void OnCollisionEnter(Collision other)
   {
      Debug.unityLogger.Log("Hitted");
      m_lightSaber.OnHitEnter(other.contacts[0].point);
   }

   private void OnCollisionExit(Collision other)
   {
      m_lightSaber.OnHitExit();
   }
}
