using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProductMadness.Minesweeper
{
    public class GridManager : MonoBehaviour
    {
        //a list for store all of unrevealed tiles
        [SerializeField] private List<Tile> m_checkedNeighbors;

        //a transform as parent of each tile game object
        [SerializeField] private Transform m_gridParent;

        //2 dimensional array for store tiles as a matrix
        [SerializeField] private Tile[,] m_grid;

        //store all of mine tiles
        [SerializeField] private List<Tile> m_mines;

        [SerializeField] private int m_remainTiles;

        private int m_gridNum;

        public void Initialize(float tileSize, int gridNum, TileTextureList tileTexturesList, Tile tilePrefab)
        {
            m_gridNum = gridNum;
            m_checkedNeighbors = new List<Tile>();
            CreateGrid(tileSize, tileTexturesList, tilePrefab);
        }

        //Create a grid of tiles and positioning of them with tile size as vector 2(x,y)
        public void CreateGrid(float tileSize, TileTextureList tileTexturesList, Tile tilePrefab)
        {
            m_grid = new Tile[m_gridNum, m_gridNum];
            float x = 0, y = 0, size = tileSize;
            //add OnGameOverEvent to Tile.GameOverEvent for Invoke it when player click on a mine
            Tile.OnGameOverEvent += OnGameOverEvent;
            //
            for (var i = 0; i < m_gridNum; i++)
            {
                x = 0;
                for (int j = 0; j < m_gridNum; j++)
                {
                    m_grid[i, j] = Instantiate(tilePrefab, m_gridParent);
                    m_grid[i, j].transform.localPosition = new Vector3(x, y);
                    m_grid[i, j].Initialize(tileTexturesList, i, j);
                    m_grid[i, j].name = $"Tile[{i},{j}]";
                    m_grid[i, j].OnRevealedTileEvent += OnUnrevealedTile;
                    m_grid[i, j].OnShowNeighboursTileEvent += ShowTileNeighbours;
                    x += size;
                }

                y -= size;
            }

            PlacingMines();


            for (var i = 0; i < m_gridNum; i++)
            {
                for (int j = 0; j < m_gridNum; j++)
                {
                    SetNeighbors(m_grid[i, j]);
                }
            }

            m_remainTiles = (m_gridNum * m_gridNum) - m_gridNum;
            Camera.main.FocusOn(m_gridParent.gameObject, 1);
        }

        private void OnGameOverEvent(Tile tile)
        {
            m_mines.Remove(tile);
            m_mines.ForEach(x => { x.Revealed(); });
        }

        private void PlacingMines()
        {
            m_mines = new List<Tile>();
            Tile rndTile;
            for (int i = 0; i < m_gridNum; i++)
            {
                int rndX = Random.Range(0, m_gridNum);
                int rndY = Random.Range(0, m_gridNum);
                rndTile = m_grid[rndX, rndY];
                rndTile.SetIsMine();
                m_mines.Add(rndTile);
            }
        }

        void SetNeighbors(Tile tile)
        {
            // first, add every possible neighbor tile position to the possible-neighbors list
            if (tile.ColIndex < m_gridNum - 1)
            {
                AddNeighbourToTile(ref tile,
                    new Neighbour(m_grid[tile.RowIndex, tile.ColIndex + 1], NeighborName.Right));
            }

            if (tile.ColIndex > 0)
            {
                AddNeighbourToTile(ref tile,
                    new Neighbour(m_grid[tile.RowIndex, tile.ColIndex - 1], NeighborName.Left));
            }

            if (tile.RowIndex > 0)
            {
                AddNeighbourToTile(ref tile,
                    new Neighbour(m_grid[tile.RowIndex - 1, tile.ColIndex], NeighborName.Up));

                if (tile.ColIndex < m_gridNum - 1)
                {
                    AddNeighbourToTile(ref tile,
                        new Neighbour(m_grid[tile.RowIndex - 1, tile.ColIndex + 1], NeighborName.UpRight));
                }

                if (tile.ColIndex > 0)
                {
                    AddNeighbourToTile(ref tile, new Neighbour(m_grid[tile.RowIndex - 1, tile.ColIndex - 1],
                        NeighborName.UpLeft));
                }
            }

            if (tile.RowIndex < m_gridNum - 1)
            {
                AddNeighbourToTile(ref tile,
                    new Neighbour(m_grid[tile.RowIndex + 1, tile.ColIndex], NeighborName.Down));

                if (tile.ColIndex < m_gridNum - 1)
                {
                    AddNeighbourToTile(ref tile, new Neighbour(m_grid[tile.RowIndex + 1, tile.ColIndex + 1],
                        NeighborName.DownRight));
                }

                if (tile.ColIndex > 0)
                {
                    AddNeighbourToTile(ref tile, new Neighbour(m_grid[tile.RowIndex + 1, tile.ColIndex - 1],
                        NeighborName.DownLeft));
                }
            }
        }

        private void AddNeighbourToTile(ref Tile mainTile, Neighbour neighbourTile)
        {
            mainTile.Neighbors.Add(neighbourTile);
            if (neighbourTile.Tile.IsMine)
            {
                mainTile.TileType = TileType.Number;
                mainTile.NeighboursMineCounter++;
            }
        }

        private void ShowTileNeighbours(Tile tile)
        {
            tile.Neighbors.Select(x => x.Tile).Where(x => x != null).ToList().ForEach(x =>
            {
                if (!x.IsMine)
                {
                    if (!m_checkedNeighbors.Contains(x))
                    {
                        if (m_checkedNeighbors.Contains(x))
                            m_checkedNeighbors.Add(x);

                        m_checkedNeighbors.Add(x);

                        x.Revealed();

                        if (x.TileType != TileType.Number)
                            ShowTileNeighbours(x);
                    }
                }
            });
        }

        private void OnUnrevealedTile()
        {
            m_remainTiles--;
            if (m_remainTiles == 0)
                Debug.unityLogger.Log("Game Over! Win");
        }
    }
}