using System.Collections.Generic;
using UnityEngine;

namespace ProductMadness.Minesweeper
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Data", order = 1)]
    public class DataScriptableObject : ScriptableObject
    {
        public Tile TilePrefab;
        public float TileSize;
        public TileTextureList TileSprites;
    }
}