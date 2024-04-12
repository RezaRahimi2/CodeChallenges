using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPrizeSelector : MonoBehaviour
{
    private Level m_manager;
    
    public void Initialize(Level manager)
    {
        m_manager = manager;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Scraper"))
            m_manager.LevelIsFinished();
    }
}