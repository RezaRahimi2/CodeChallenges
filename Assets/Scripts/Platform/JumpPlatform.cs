using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JumpPlatform : Platform
{
   [SerializeField] private GameObject m_mainGameObject;
   
   //Used for if the platform is destroyable
   [Header("Destroyable")]
   [SerializeField] private bool m_isDestroyable;
   //after number of hit, reach the below value platform will be destroyed 
   [SerializeField] private int m_destroyHitCount;
   [SerializeField] private DestroyableObject m_destroyableObject;

   [Space] [SerializeField] private LevelNameObject m_levelNameObject;
   [SerializeField] private List<SibelPart> m_sibelParts;
   [SerializeField] private int repeatNumber;
   [SerializeField] private Trampoline m_trampoline;
   //used for my first object design, platforms were a UFO spaceship 
   [SerializeField] private UFO m_ufo;
   //stored the hit particle of platform
   [SerializeField] private ParticleSystem m_hitParticle;
   [SerializeField] private ParticleSystem.MainModule particleMainModule;

   //stored the tween animations 
   private Tween m_scaleTween;
   private Tween m_rotateTween;
   
   public float ForceToPlayer;
   private PlatformManager m_manager;
   
   //a counter, used for repeat the scale changing by hit 
   private int repeatCounter;
   
   //Initialize method for jump platform, get the number of the platform from Platform manager
   public override void Initialize(int platformNum,PlatformManager manager)
   {
      m_manager = manager;
      particleMainModule = m_hitParticle.main;
      //set -1 to high jump platform for don't show then in progress bar of level
      if (!HighJumpPlatform)
         PlatformNum = platformNum;
      else
         PlatformNum = -1;
      //set the platform number to Level name object text mesh
      m_levelNameObject.Initialize(platformNum.ToString());
      //Initialize the sibel parts of the jump platform
      m_sibelParts.ForEach(x =>
      {
         x.Initialize(this);
      });
      // initialize the destroyable object
      if(m_destroyableObject !=null)
         m_destroyableObject.Initialize();

      // initialize the UFO object from my first design
      if (m_ufo != null)
      {
         m_ufo.Initialize(m_destroyHitCount, this);
         m_ufo.transform.DORotate(new Vector3(0, 360, 0), .5f, RotateMode.FastBeyond360).SetEase(Ease.Flash)
            .SetLoops(-1, LoopType.Incremental);
      }
      
      //Initialize the trampoline object if is destroyable 
      if (m_trampoline != null)
      {
         m_trampoline.Initialize(m_destroyHitCount,this);
      }
   }

   //for start hit animation of the sibel
   public void StartHitAnimation(float hitValue)
   {
      bool isDestroy = false;
      repeatCounter = 0;
      if (m_isDestroyable)
      {
         if(m_ufo != null)
            m_ufo.ShowFire();
         else if(m_trampoline != null)
          isDestroy = m_trampoline.ShowDestroyable();
      }
      if(!isDestroy)
         HitAnimation(hitValue);
      
      if (HighJumpPlatform)
      {
         DOVirtual.DelayedCall(.75f, () =>
         {
            m_scaleTween.Kill();
            m_rotateTween.Kill();
            transform.DOMoveY(transform.position.y - 400, 3);
            transform.DOLocalRotate(new Vector3(0, 0, 720), 3,RotateMode.FastBeyond360).OnComplete(() =>
            {
               Destroy(gameObject);
            });
         });
         
      }

   }
   
   //hit animation of the sibel 
   public void HitAnimation(float hitValue)
   {
      float delay = 0f;
      int index = 0;
      m_sibelParts.ForEach(x =>
      {
         SibelPartAnimation(x, hitValue,delay,index++, (_index) =>
         {
            if (_index == m_sibelParts.Count - 1 && repeatCounter <= repeatNumber)
            {
               hitValue -= .05f;
               HitAnimation(hitValue);
               repeatCounter++;
            }
         });
         delay += .005f;
      });
   }

   //a method for tween each sibel part scale and Y axis move
   private void SibelPartAnimation(SibelPart sibelPart,float hitValue, float delay,int sibelPartIndex,Action<int> afterDoneCallback)
   {
      m_scaleTween = sibelPart.transform.DOScale(sibelPart.StartScaleSize * 1.2f, .2f).OnComplete(() =>
      {
         sibelPart.transform.DOScale(sibelPart.StartScaleSize, .1f);
      });
      m_rotateTween = sibelPart.transform.DOMoveY(sibelPart.StartYValue - hitValue, .1f).SetDelay(delay).OnComplete(() =>
      {
         m_rotateTween = sibelPart.transform.DOMoveY(sibelPart.StartYValue, .1f)
            .OnComplete(() =>
            {
               afterDoneCallback.Invoke(sibelPartIndex);
            });
      });
   }

   //called when player hit to platform collider
   public void HitToPlatform(Transform playerTransform)
   {
      StartHitAnimation(.1f);
      particleMainModule.scalingMode = ParticleSystemScalingMode.Local;
      particleMainModule.startSize = 1;
      m_hitParticle.transform.DOScale(new Vector3(49, 49, 0), 1).OnComplete(() =>
      {
         m_hitParticle.transform.localScale = Vector3.zero;
         particleMainModule.startSize = 0; 
      });
      m_manager.HitToPlatform(PlatformNum,playerTransform,ForceToPlayer);
   }

   //called when the platform must be destroy
   public void ShowDestruction()
   {
      m_mainGameObject.SetActive(false);
      m_destroyableObject.EnableRigidBody();
      DOVirtual.DelayedCall(5, () =>
      {
         DOTween.KillAll();
         Destroy(gameObject);   
      });
   }
   
   //Show fragmented object, before one hit remain to destroy 
   public void ShowDestroyableObject()
   {
      m_mainGameObject.SetActive(false);
      m_destroyableObject.gameObject.SetActive(true);
      
   }
}
