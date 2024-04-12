using System;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    //Custom editor for add spin button and reset button to inspector of LuckyWheelController class
    [CustomEditor(typeof(LuckyWheelController))]
    public class LuckyWheelEditor : UnityEditor.Editor
    {
        private LuckyWheelController _mLuckyWheelController;

        private void OnEnable()
        {
            _mLuckyWheelController = target as LuckyWheelController;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Spin"))
            {
                _mLuckyWheelController.Spin();
            }
            if (GUILayout.Button("Reset"))
            {
                _mLuckyWheelController.Reset();
            }
        }
    }
}