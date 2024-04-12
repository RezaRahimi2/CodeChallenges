using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainObject : NonInteractiveObject
{
    public List<Platform> Platforms;

    public void SetColor(Color color)
    {
        Renderer.sharedMaterial.color = color;
    }
}
