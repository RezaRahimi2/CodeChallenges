 using System.Collections.Generic;
 using UnityEngine;

 //Stored the platform layer mask and move and rotate speed
 [CreateAssetMenu(fileName = "GameManagerData", menuName = "GameManagerData", order = 1)]
    public class GameManagerData : ScriptableObject
    {
        public float MoveSpeed;
        public List<Color> CollectableCubeColorSet;
        public List<Color> FinishPlatformColorSet;
        public List<Color> ObstacleCubeColorSet;
    }