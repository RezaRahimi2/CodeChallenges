using System;
using Immersed.General;
using UnityEngine;
using UnityEngine.UI;

public class ButtonWithRequestName : MonoBehaviour,IRequestNameWithApiVersion
{
    [field: SerializeField]public RequestNameWithApiVersion RequestNameWithApiVersion { get; set; }
    [SerializeField]private Button m_button;

    public void Initialize(Action onClick)
    {
        m_button = GetComponent<Button>();
        AddOnClickListener(onClick);
    }
    
    private void AddOnClickListener(Action onclick)
    {
        m_button.onClick.AddListener(()=>onclick?.Invoke());
    }
}
