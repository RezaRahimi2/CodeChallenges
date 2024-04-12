using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.GameOver();
    }
}
