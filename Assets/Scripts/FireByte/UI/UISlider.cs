using System;
using System.Collections;
using System.Collections.Generic;
using FireByte;
using UnityEngine;
using UnityEngine.UI;

public class UISlider : MonoBehaviour
{
    private UIManager m_manager;

    [SerializeField] private Slider m_slider;
    [SerializeField] private Text m_label;
    [SerializeField] private LightSaberName m_lightSaberName;
    [SerializeField] private RotateDirection m_rotateDirection;

    public void Initialize(UIManager manager,bool aiEnabled)
    {
        //if AI active the slider of blue light saber be deactive
        if (aiEnabled && m_lightSaberName == LightSaberName.Blue)
            m_slider.interactable = false;    
        
        m_manager = manager;
        //set initial value of slider
        m_slider.value = 0;
        //add a event to on value change
        m_slider.onValueChanged.AddListener(OnValueChange);
    }
    
    //removing all listener of slider 
    public void RemoveOnValueChangeListener()
    {
        m_slider.onValueChanged.RemoveAllListeners();
    }

    private void OnValueChange(Single value)
    {
        m_manager.OnSliderValueChange(m_lightSaberName, m_rotateDirection, value);
    }
}