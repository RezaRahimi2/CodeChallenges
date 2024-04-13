using System.Collections;
using System.Collections.Generic;
using Immersed.General;
using UnityEngine;

namespace Immersed.Task4
{
    //UI manager of Task 4
    public class UIManager : MonoBehaviour
    {
        //Instance of UserModelUi of Task 4
        [SerializeField]private GuestUserModelUI m_guestUserModelUI;
        [SerializeField]private RegisteredUserModelUI m_registeredUserModelUI;

        public void SetUserModelToUI(UserClass userClass,UserModel userModel)
        {
            HideAllView();
            if (userClass == UserClass.Guest)
            {
                m_guestUserModelUI.gameObject.SetActive(true);
                m_guestUserModelUI.SetView(userModel);
            }
            else
            {
                m_registeredUserModelUI.gameObject.SetActive(true);
                m_registeredUserModelUI.SetView(userModel);
            }
        }

        public void SetProfileImage(Texture2D texture2D)
        {
            m_registeredUserModelUI.SetProfileImage(texture2D);
        }
        
        public void HideAllView()
        {
            m_guestUserModelUI.gameObject.SetActive(false);
            m_registeredUserModelUI.gameObject.SetActive(false);
        }
        
    }
}