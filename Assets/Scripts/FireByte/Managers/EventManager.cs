using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FireByte
{
    public class EventManager : Manager
    {
        public LightSaberRotateEvent OnLightSaberRotateEvent;
        public HitEvent OnHitEvent;
        public SimulateEvent OnSimulateEvent;
        public GameOverEvent OnGameOverEvent;
        
        public override void Initialize()
        {
            OnLightSaberRotateEvent = null;
            OnHitEvent = null;
            OnSimulateEvent = null;
            OnGameOverEvent = null;
        }
    }
}