using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PrizeManager : MonoBehaviour
{
   [SerializeField] private List<Prize> m_prizes;
   
   public void Initialize()
   {
      m_prizes = FindObjectsOfType<Prize>().ToList();
      
      m_prizes.ForEach(x =>
      {
         x.Initialize(this);
      });
   }

   public void DisableOtherPrizes(Prize hittedPrize)
   {
      m_prizes.FindAll(x=> !x.Equals(hittedPrize)).ForEach(x =>
      {
         x.BoxCollider.enabled = false;
      });
   }
}
