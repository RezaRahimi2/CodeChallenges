using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

//Each level have colors attributes for different segment of game with Platform Data
[Serializable]
public class Level
{
    [SerializeField]
    [DefaultValue(typeof(Color), "Black")]
    public JsonColor BackgroundColor;
    
    [SerializeField]
    [DefaultValue(typeof(Color), "Black")]
    public JsonColor MainBarColor;
    
    [SerializeField]
    [DefaultValue(typeof(Color), "Black")]
    public JsonColor GoalPlatformColor;
    
    [SerializeField]
    [DefaultValue(typeof(Color), "Black")]
    public JsonColor FoulPlatformColor;
    
    [SerializeField]
    [DefaultValue(typeof(Color), "Black")]
    public JsonColor NeutralPlatformColor;
    
    [SerializeField]
    [DefaultValue(typeof(Color), "Black")]
    public JsonColor BallColor;
    
    [SerializeField]
    public int BallMass;
    [SerializeField]
    public PlatformData[] PlatformData = new PlatformData[14];
}
