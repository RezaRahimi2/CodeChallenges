using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProductMadness.Minesweeper
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private bool m_isRevealed;
        [SerializeField] private bool m_isMine;
        [SerializeField] private bool m_showFlag;
        public bool IsMine => m_isMine;

        [SerializeField] private TileType m_tileType;

        public TileType TileType
        {
            set
            {
                m_tileType = value;
            }
            get
            {
                return m_tileType;
            }
        }

        [SerializeField] private Renderer m_rederer;

        public int NeighboursMineCounter;

        [SerializeField] private TileTextureList m_tileTextureList;

        public Action OnRevealedTileEvent;
        public Action<Tile> OnShowNeighboursTileEvent;
        public static Action<Tile> OnGameOverEvent;

        public List<Neighbour> Neighbors;

        [SerializeField] private int m_colIndex;
        [SerializeField] private int m_rowIndex;

        public int ColIndex => m_colIndex;
        public int RowIndex => m_rowIndex;

        public void Initialize(TileTextureList tileTexturesList, int rowIndex, int colIndex)
        {
            m_colIndex = colIndex;
            m_rowIndex = rowIndex;
            m_tileTextureList = tileTexturesList;
            m_rederer.material.mainTexture = m_tileTextureList.GetTexture(TileType.Revealed);
        }

        public void SetIsMine()
        {
            m_tileType = TileType.Mine;
            m_isMine = true;
        }

        private void OnMouseOver()
        {
            if (!m_isRevealed)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Revealed();
                    if (m_isMine)
                        OnGameOverEvent?.Invoke(this);
                    m_isRevealed = true;
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    if (!m_showFlag)
                    {
                        ShowFlag();
                    }
                    else
                    {
                        HideFlag();
                    }

                    m_showFlag = !m_showFlag;
                }
            }
        }

        public void Revealed()
        {
            if (m_isMine)
            {
                m_rederer.material.mainTexture = m_tileTextureList.GetTexture(TileType.Mine);
            }
            else if (m_tileType == TileType.Number)
            {
                m_rederer.material.mainTexture = m_tileTextureList.GetTextureNumber(NeighboursMineCounter);
                OnRevealedTileEvent.Invoke();
            }
            else
            {
                m_tileType = TileType.Unrevealed;
                m_rederer.material.mainTexture = m_tileTextureList.GetTexture(TileType.Unrevealed);
                OnShowNeighboursTileEvent?.Invoke(this);
                OnRevealedTileEvent.Invoke();
            }
        }

        public void ShowFlag()
        {
            m_rederer.material.mainTexture = m_tileTextureList.GetTexture(TileType.Flag);
        }
        
        public void HideFlag()
        {
            m_rederer.material.mainTexture = m_tileTextureList.GetTexture(TileType.Revealed);
        }
    }
}