using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Prize : MonoBehaviour
{
    private float startTime;
    private float endTime;
    public BoxCollider BoxCollider;
    private PrizeManager _manager;

    public void Initialize(PrizeManager prizeManager)
    {
        _manager = prizeManager;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spiral"))
            startTime = Time.time;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Spiral"))
        {
            endTime = Time.time;

            if (endTime - startTime > 1)
            {
                _manager.DisableOtherPrizes(this);
                transform.DOMoveX(transform.position.x - .5f, 1);
            }
        }
    }
}