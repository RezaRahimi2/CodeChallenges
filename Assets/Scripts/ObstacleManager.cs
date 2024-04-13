using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] private List<ObstacleGroup> m_obstaclesGroups;
    [SerializeField] private FinishPlatform[] m_finishPlatforms;

    public void Initialize(List<Color> obstacleCubeColorSet, List<Color> finishPlatformColors)
    {
        m_obstaclesGroups = FindObjectsOfType<ObstacleGroup>().ToList();

        m_obstaclesGroups.ForEach(x => { x.Initialize(this, obstacleCubeColorSet); });
        for (var i = 0; i < m_finishPlatforms.Length; i++)
        {
            m_finishPlatforms[i].Initialize(this, finishPlatformColors[i]);
        }
    }

    public void HitToObstacle(PlayerCube playerCube, int obstacleCubeCount, ObstacleCube obstacleCube)
    {
        playerCube.transform.parent = null;
        playerCube.RemoveRigidBody();
        GameManager.Instance.HitToObstacle(playerCube, obstacleCubeCount, obstacleCube);
    }

    public void HitToFinishPlatform(PlayerCube playerCube)
    {
        playerCube.transform.parent = null;
        playerCube.RemoveRigidBody();
        GameManager.Instance.HitToFinishPlatform(playerCube);
    }
}