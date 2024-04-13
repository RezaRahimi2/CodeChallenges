using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCube : Cube<ObstacleGroup>
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("PlayerCube"))
            m_manager.HitToObstacle(other.transform.GetComponent<PlayerCube>(),this);
    }
}