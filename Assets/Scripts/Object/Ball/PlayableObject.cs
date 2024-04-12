using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayableObject : Object
{
    public Rigidbody Rigidbody { private set; get; }
    public Material Material;
    
    public virtual void Initialize(int mass,Color color)
    {
        Rigidbody = GetComponent<Rigidbody>();
        Material = GetComponent<Renderer>().sharedMaterial;
        
        Material.color = color;
        Rigidbody.mass = mass;
    }
}
