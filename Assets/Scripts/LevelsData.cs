using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelsData", menuName = "LevelsData", order = 1)]
public class LevelsData : ScriptableObject
{
    public List<GameObject> LevelsPrefab;
    
}