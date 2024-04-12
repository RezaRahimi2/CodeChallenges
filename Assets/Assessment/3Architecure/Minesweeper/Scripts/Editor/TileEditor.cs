using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Minesweeper.Scripts.Editor
{
    [CustomEditor(typeof(ProductMadness.Minesweeper.Tile))]
    public class TileEditor : UnityEditor.Editor
    {
        private ProductMadness.Minesweeper.Tile m_tile;

        public void OnEnable()
        {
            m_tile = target as ProductMadness.Minesweeper.Tile;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Select Neighbours"))
            {
                Selection.objects = m_tile.Neighbors.Select(x => x.Tile.gameObject).ToArray();
            }
        }
        
    }
}