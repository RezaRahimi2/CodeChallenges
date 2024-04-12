using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used for controlling a player
public class PlayerController : MonoBehaviour
{
   [SerializeField] private Player m_player;

   //initialize the player
   public void Initialize(Transform playerStartTransform,LayerMask platformLayerMask)
   {
      m_player.Initialize(playerStartTransform.position,playerStartTransform.eulerAngles,platformLayerMask);
   }

   //add force to player
   public void AddForceToPlayer(float force)
   {
      m_player.AddForce(force);
   }
   
   //change rotate of the player
   public void RotatePlayer(float angle)
   {
      m_player.transform.localEulerAngles += new Vector3(0,angle,0);
   }

   //change position of the player
   public void MovePlayer(float value)
   {
      m_player.transform.Translate((Vector3.forward * value));
   }

   public void LevelIsFinished()
   {
      m_player.FinishLevel();
   }
}
