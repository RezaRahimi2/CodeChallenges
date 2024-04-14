///////////////////////////////////////////////////////////
//  UIManager.cs
//  Implementation of the Class UIManager
//  Generated by Enterprise Architect
//  Created on:      30-May-2021 2:56:55 PM
//  Original author: Reza
///////////////////////////////////////////////////////////


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FireByte
{
    public class UIManager : Manager<float,bool>
    {
        [SerializeField] private UIButton m_simulateButton;
        [SerializeField] private List<UISlider> m_uiSliders;
        [SerializeField] private GameOverPopup m_gameOverPopup;

        public override void Initialize(float fadeDuration,bool aiEnabled)
        {
            SimulateButtonInit();
            m_uiSliders.ForEach(x =>
            {
                x.Initialize(this,aiEnabled);
            });
            m_gameOverPopup.Initialize(fadeDuration);
        }
        
        public void OnSliderValueChange(LightSaberName lightSaberName, RotateDirection rotateDirection, float value)
        {
            GameManager.Instance.OnRotateChange(lightSaberName, PlayerType.Player, rotateDirection, value);
        }

        public void ShowGameOverPopup()
        {
            m_uiSliders.ForEach(x => x.RemoveOnValueChangeListener());
            m_gameOverPopup.Show();
        }

        public void SimulateButtonInit()
        {
            m_simulateButton.SetText("Simulate");
            m_simulateButton.SetButtonOnClickEvent(true,onSimulateButtonClick);
        }
        
        public void ChangeSimulateButtonToResetButton()
        {
            m_simulateButton.SetText("Reset");
            m_simulateButton.SetButtonOnClickEvent(true,onResetButtonClick);
        }
        
        private void onSimulateButtonClick()
        {
            GameManager.Instance.Simulate();
        }

        private void onResetButtonClick()
        {
            GameManager.Instance.ResetToStart();
        }
    } //end UIManager
} //end namespace FireByte