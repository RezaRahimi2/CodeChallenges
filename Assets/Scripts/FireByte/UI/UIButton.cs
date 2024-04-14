using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    [SerializeField] private Button m_button;
    [SerializeField] private Text m_text;

    public void SetButtonOnClickEvent(bool removeOnClickListener,Action action)
    {
        if(removeOnClickListener)
            m_button.onClick.RemoveAllListeners();
        
        m_button.onClick.AddListener(action.Invoke);
    }

    public void SetText(string textStr)
    {
        m_text.text = textStr;
    }
}
