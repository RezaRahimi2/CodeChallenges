using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Level : MonoBehaviour
{
    public Transform PlayerStartTransform;
    public int LevelNum;
    public List<Platform> Platforms;
}
