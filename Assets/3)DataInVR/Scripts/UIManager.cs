using System.IO;
using DG.Tweening;
using Immersed.General;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Immersed.Task3
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private UserModelUI m_userModelUI;

        public void Initialize()
        {
            m_userModelUI.Initialize(OnClickLoginButton);
            RequestManager.Instance.OnResponseUserEvent.OnResponse += OnResponseEvent;
        }

        private void OnResponseEvent(string response, UserModel userModel, bool status)
        {
            if (status)
            {
                if (userModel.ProfilePicUrl != null)
                {
                    Request requestClass = RequestManager.Instance.GetProfilePicRequest(userModel.ProfilePicUrl,
                        async (status, profilePicBytes, text, message, code) =>
                        {
                            Texture2D texture2d = new Texture2D(150, 150);
                            texture2d.LoadImage(profilePicBytes);
                            byte[] _bytes = ImageConversion.EncodeArrayToJPG(texture2d.GetRawTextureData(),
                                texture2d.graphicsFormat, (uint) texture2d.width, (uint) texture2d.height);
                            File.WriteAllBytes(DataManager.Data.GetLocalProfilePicPath(), _bytes);
                            m_userModelUI.ShowProfileImage(texture2d);
                        });
                    RequestBundle bndleReq = new RequestBundle(requestClass);
                    ServerAccess.Instance.SendBatchRequest(bndleReq);
                }

                m_userModelUI.OnLogin(true, userModel);
            }
            else
            {
                m_userModelUI.OnLogin(false, null);
            }
        }

        public void OnClickLoginButton()
        {
            Request requestClass =
                RequestManager.Instance.GetRequest(new RequestNameWithApiVersion(RequestName.EmailLogin,
                    APIVersion.V1));
            requestClass.AddBody("email", m_userModelUI.EmailInput);
            requestClass.AddBody("password", m_userModelUI.PasswordInput);
            RequestBundle bndleReq = new RequestBundle(requestClass);
            ServerAccess.Instance.SendBatchRequest(bndleReq);
        }
    }
}