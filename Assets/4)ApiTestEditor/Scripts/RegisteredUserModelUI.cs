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
    public class RegisteredUserModelUI : UserModelView
    {
        [SerializeField] private Image m_profileImage;
        [SerializeField] private TextMeshProUGUI m_emailTextMeshValue;
        [SerializeField] private Toggle m_userstatusToggleValue;
        [SerializeField] private TextMeshProUGUI m_userclassTextMeshValue;


        [SerializeField] private Transform m_linksContentParent;
        [SerializeField] private TextMeshProUGUI m_linkTextPrefab;


        public void SetProfileImage(Texture2D texture2d)
        {
            if (texture2d != null)
                m_profileImage.sprite =
                    Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), Vector2.zero);
            else
                m_profileImage.sprite = null;
        }

        public override void SetView(UserModel userModel)
        {
                m_emailTextMeshValue.text = userModel.email;
                m_userstatusToggleValue.isOn = userModel.UserStatus;
                m_userclassTextMeshValue.text = userModel.UserClass.ToString();
                
                if (userModel.Links != null)
                {
                    for (int i = 0; i < m_linksContentParent.childCount; i++)
                    {
                        DestroyImmediate(m_linksContentParent.GetChild(i).gameObject);
                    }
                    
                    foreach (string userModelLink in userModel.Links)
                    {
                        TextMeshProUGUI textMeshProUGUI =
                            Instantiate<TextMeshProUGUI>(m_linkTextPrefab, m_linksContentParent);
                        textMeshProUGUI.text = userModelLink;
                    }

                    Instantiate<TextMeshProUGUI>(m_linkTextPrefab, m_linksContentParent);
                }
        }
    }
}