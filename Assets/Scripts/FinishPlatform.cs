using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPlatform : Cube<ObstacleManager>
{
    [SerializeField] private bool hitted;
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("PlayerCube") && !hitted)
        {
            hitted = true;
            m_manager.HitToFinishPlatform(other.transform.GetComponent<PlayerCube>());
        }
    }
}
