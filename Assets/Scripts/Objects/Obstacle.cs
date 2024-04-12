using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private ObstacleManager _manager;
    private BoxCollider _boxCollider;
    private Rigidbody _rigidbody;
    [SerializeField]private bool m_isStaticObstacle;

    public void Initialize(ObstacleManager manager)
    {
        _boxCollider = GetComponent<BoxCollider>();
        _rigidbody = GetComponent<Rigidbody>();
        _manager = manager;
    }

    public void EnableGravity()
    {
        if (!m_isStaticObstacle)
        {
            _rigidbody.useGravity = true;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Scraper"))
            _manager.HitToObstacle();
        else if (other.transform.CompareTag("Spiral") && !m_isStaticObstacle)
        {
            _boxCollider.enabled = false;
            _rigidbody.constraints = RigidbodyConstraints.None;

        }
    }
}