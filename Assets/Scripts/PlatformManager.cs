using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//used for managing the jump platforms
public class PlatformManager : MonoBehaviour
{
    //stored the last platform, get the last platform from the platform list as Final platform 
    [SerializeField] public FinalPlatform FinalPlatform => m_platforms.Last() as FinalPlatform;
    //stored the list of platfoms
    [SerializeField] private List<Platform> m_platforms;

    public void Initialize(List<Platform> jumpPlatforms)
    {
        m_platforms = jumpPlatforms;
        int levelNameCounter = 0;
        m_platforms.ForEach(x => { x.Initialize(levelNameCounter++, this); });
    }

    //Called when player hit to platform and notify the game manager
    public void HitToPlatform(int platformNum, Transform player, float force)
    {
        GameManager.Instance.HitToPlatform(platformNum, player, force);
    }

    //called when player hit to final platform
    public void HitToLastPlatform(int platformNum)
    {
        GameManager.Instance.LevelIsFinished(platformNum);
    }
}