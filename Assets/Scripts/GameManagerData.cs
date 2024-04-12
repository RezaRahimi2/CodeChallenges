 using UnityEngine;

 //Stored the platform layer mask and move and rotate speed
 [CreateAssetMenu(fileName = "GameManagerData", menuName = "GameManagerData", order = 1)]
    public class GameManagerData : ScriptableObject
    {
        public LayerMask PlatformLayerMask;

        public float MoveSpeed;
        public float RotateSpeed;
    }