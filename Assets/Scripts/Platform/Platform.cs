using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base class of the Jump platform
public abstract class Platform : MonoBehaviour
{
    [Header("High Jump")] public bool HighJumpPlatform;

    public int PlatformNum;
    public PlatformManager Manager;

    public abstract void Initialize(int platformNum, PlatformManager platformManager);
}
