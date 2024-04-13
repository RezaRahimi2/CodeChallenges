using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ObstacleGroup : GroupBase<ObstacleManager,ObstacleCube>
{
    public override void Initialize(Color color)
    {
        m_cubes.ForEach(x =>
        {
            x.Initialize(this,color);
        });
    }
    
    public void HitToObstacle(PlayerCube playerCube,ObstacleCube obstacleCube)
    {
        //if (!m_hitCoolDown)
        {
            m_manager.HitToObstacle(playerCube,m_cubes.Count,obstacleCube);
        }
    }
    
    

   
}
