using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIPopUp : MonoBehaviour
{
    public abstract void Initialize(float fadeDuration);
    
    public abstract void Show();
    public abstract void Hide();
}
