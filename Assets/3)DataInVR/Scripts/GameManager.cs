using System;
using UnityEngine;

namespace Immersed.Task3
{
    

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager m_uiManager;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        m_uiManager.Initialize();
    }

    private void FindReferences()
    {
        m_uiManager = FindObjectOfType<UIManager>();
    }
}

}
