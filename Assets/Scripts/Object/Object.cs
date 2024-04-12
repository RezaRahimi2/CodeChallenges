using UnityEngine;

//Parent class of all playable(interactive and non interactive) Objects in game 
public abstract class Object : MonoBehaviour
{
    public Color Color
    {
        set
        {
            GetComponent<Renderer>().sharedMaterial.color = value;
        }
        get => Color;
    }
}
