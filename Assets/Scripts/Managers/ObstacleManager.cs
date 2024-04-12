using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] private List<Obstacle> m_obstacles;
    
    public void Initialize()
    {
        m_obstacles = FindObjectsOfType<Obstacle>().ToList();
        m_obstacles.ForEach(x=> x.Initialize(this));
    }

    public void EnableObstaclesGravity()
    {
        m_obstacles.ForEach(x => x.EnableGravity());
    }
    
    public void HitToObstacle()
    {
        GameManager.Instance.HitToObstacle();
    }
}
