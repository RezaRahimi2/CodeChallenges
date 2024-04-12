using System;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public struct PlatformData
{
    [SerializeField] public bool IsActive;
    [Range(0, 11)] [SerializeField] public int NeutralObjectsNumber;
    [Range(0, 11)] [SerializeField] public int GapObjectsNumber;
    [Range(0, 11)] [SerializeField] public int FoulObjectNumbers;
}


[Serializable]
public struct JsonColor
{
    [SerializeField][HideInInspector] public float _r;
    [SerializeField][HideInInspector] public float _g;
    [SerializeField][HideInInspector] public float _b;
    [SerializeField][HideInInspector] public float _a;

    [JsonIgnore]private Color color;
    [JsonIgnore]
    public Color Color
    {
        get { return color = new Color(_r, _g, _b, _a); }
        set
        {
            _r = value.r;
            _g = value.g;
            _b = value.b;
            _a = value.a;
        }
    }
}