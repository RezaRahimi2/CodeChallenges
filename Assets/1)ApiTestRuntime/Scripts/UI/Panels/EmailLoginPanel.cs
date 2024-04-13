using System.IO;
using Immersed.General;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Immersed.Task1
{
    public class EmailLoginPanel : PanelWithRequestName
    {
        [SerializeField] private TMP_InputField m_emailInput;
        [SerializeField] private TMP_InputField m_passwordInput;
        [SerializeField] private Image m_profileImageValue;
        [SerializeField] private Toggle m_userStatusValue;
        [SerializeField] private TextMeshProUGUI m_userClassTextValue;
        [SerializeField] private TextMeshProUGUI m_nicknameTextValue;
        [SerializeField] private Toggle m_paidUserValue;


        public override void Show()
        {
            base.Show();
        }

        public override void SendRequest()
        {
            base.SendRequest();
            Request requestClass = RequestManager.Instance.GetRequest(RequestNameWithApiVersion);
            requestClass.AddBody("email", m_emailInput.text);
            requestClass.AddBody("password", m_passwordInput.text);
            RequestBundle bndleReq = new RequestBundle(requestClass);
            ServerAccess.Instance.SendBatchRequest(bndleReq);
        }

        public override void OnResponse<T>(T responseData)
        {
            UserModel userModel = (responseData as UserModel);

            m_userStatusValue.isOn = userModel.UserStatus;
            m_userClassTextValue.text = userModel.UserClass.ToString();
            m_paidUserValue.isOn = userModel.PaidUser;
            m_nicknameTextValue.text = userModel.Nickname;

            if (userModel.ProfilePicUrl != null)
            {
                Request requestClass = RequestManager.Instance.GetProfilePicRequest(userModel.ProfilePicUrl,
                     async (status, profilePicBytes, text, message, code) =>
                    {
                        Debug.Log(profilePicBytes);
                     
                        Texture2D texture2d = new Texture2D(150, 150);
                        texture2d.LoadImage(profilePicBytes);
                        byte[] _bytes = ImageConversion.EncodeArrayToJPG(texture2d.GetRawTextureData(), texture2d.graphicsFormat, (uint)texture2d.width, (uint)texture2d.height);
                        File.WriteAllBytes(DataManager.Data.GetLocalProfilePicPath(), _bytes);
                        m_profileImageValue.sprite =
                            Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), Vector2.zero);
                    });
                RequestBundle bndleReq = new RequestBundle(requestClass);
                ServerAccess.Instance.SendBatchRequest(bndleReq);
            }
        }
    }
}