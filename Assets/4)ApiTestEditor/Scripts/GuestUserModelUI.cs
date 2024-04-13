using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Immersed.General;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Immersed.Task4
{
    //GuestUserModelUi of Task 4
    public class GuestUserModelUI : UserModelView
    {
        [SerializeField] private TextMeshProUGUI m_uidTextMeshValue;
        [SerializeField] private Toggle m_userstatusToggleValue;
        [SerializeField] private TextMeshProUGUI m_userclassTextMeshValue;

        public override void SetView(UserModel userModel)
        {
            m_uidTextMeshValue.text = userModel.UserID.ToString();
            m_userstatusToggleValue.isOn = userModel.UserStatus;
            m_userclassTextMeshValue.text = userModel.UserClass.ToString();
        }
    }
}