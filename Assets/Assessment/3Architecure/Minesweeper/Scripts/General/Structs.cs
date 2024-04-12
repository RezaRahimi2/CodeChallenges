using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace ProductMadness.Minesweeper
{
    [Serializable]
    public struct TileTexture
    {
        [SerializeField] public TileType TileType;
        [SerializeField] public Texture Texture;
    }

    [Serializable]
    public struct TileTextureList
    {
        [SerializeField] public List<TileTexture> TileTextures;
        [SerializeField] public List<Texture> TileNumbers;
        public Texture GetTexture(TileType tileType)
        {
            return TileTextures.Find(x => x.TileType == tileType).Texture;
        }

        public Texture GetTextureNumber(int number)
        {
            return TileNumbers[number - 1];
        }
    }

    [Serializable]
    public struct Neighbour
    {
        [SerializeField]
        public Tile Tile;
        [SerializeField]
        public NeighborName NeighborName;

        public Neighbour(Tile tile, NeighborName neighborName)
        {
            Tile = tile;
            NeighborName = neighborName;
        }
    }
}