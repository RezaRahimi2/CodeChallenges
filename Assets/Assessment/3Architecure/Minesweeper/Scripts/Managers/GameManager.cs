using System;
using UnityEngine;

namespace ProductMadness.Minesweeper
{
    public class GameManager : MonoBehaviour
    {
        //for set grid size
        [SerializeField] private int m_gridSize;
        //Instance of the scriptable object of data for get the tile prefab and textures
        [SerializeField] private DataScriptableObject m_data;
        //Instance of grid manager
        [SerializeField] private GridManager m_gridManager;
        private void Awake()
        {
            Initialize(m_gridSize);
        }

        public void Initialize(int gridNum)
        {
            m_gridManager.Initialize(m_data.TileSize,gridNum,m_data.TileSprites,m_data.TilePrefab);
        }

    }
}