using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPlatform : Platform
{
    public List<ParticleSystem> VictoryParticles; 
    private void OnCollisionEnter(Collision other)
    {
        Manager.HitToLastPlatform(PlatformNum);
    }

    public override void Initialize(int platformNum, PlatformManager platformManager)
    {
        PlatformNum = platformNum;
        Manager = platformManager;
    }
}
 