using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events
{
    public delegate void AddCubeEvent(CollectableCube collectableCube);
    public AddCubeEvent OnAddCubeEvent;
    
    public delegate void HitToObstacleCubeEvent(PlayerCube playerCube,ObstacleCube obstacleCube);
    public HitToObstacleCubeEvent OnHitToObstacleCubeEvent;

    public delegate void HitToObstacleRunOneTime();
    public HitToObstacleRunOneTime OnHitToObstacleRunOneTime;

    public delegate void HitToFinishPlatform(PlayerCube playerCube);
    public HitToFinishPlatform OnHitToFinishPlatform;
    
    public delegate void GameIsOverEvent(bool playerIsWinner);
    public GameIsOverEvent OnGameIsOverEvent;


}
