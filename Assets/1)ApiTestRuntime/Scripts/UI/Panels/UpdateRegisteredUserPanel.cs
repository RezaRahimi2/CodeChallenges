using System.IO;
using Immersed.General;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Immersed.Task1
{
    public class UpdateRegisteredUserPanel : PanelWithRequestName
    {
        [SerializeField] private TextMeshProUGUI m_authenticationTokenValue;
        [SerializeField] private TMP_InputField m_passwordInput;
        [SerializeField] private TMP_InputField m_nicknameInput;
        [SerializeField] private TextMeshProUGUI m_emailValue;
        [SerializeField] private Image m_profilePicValue;
        [SerializeField] private Button m_profilePicOpenButton;
        byte[] profilePicBytes;

        public override void Initialize()
        {
            base.Initialize(OpenImageCallBack);
            m_profilePicOpenButton.onClick.AddListener(OpenFile);
        }

        public override void Show()
        {
            base.Show();
            m_profilePicValue.sprite = null;
            if (PlayerPrefs.HasKey(DataManager.Data.PREFS_SECRET_KEY))
            {
                string encrypted = PlayerPrefs.GetString(DataManager.Data.PREFS_SECRET_KEY);
                m_authenticationTokenValue.text = AES.Decrypt(encrypted);
                if (PlayerPrefs.HasKey(DataManager.Data.PREFS_PROFILE_DATA))
                {
                    encrypted = PlayerPrefs.GetString(DataManager.Data.PREFS_PROFILE_DATA);
                    UserModel userModelData = JsonConvert.DeserializeObject<UserModel>(AES.Decrypt(encrypted, "ud"));
                    if (userModelData.UserClass == UserClass.Registered)
                    {
                        m_nicknameInput.text = userModelData.Nickname;
                        m_emailValue.text = userModelData.email;
                        if (!string.IsNullOrEmpty(userModelData.ProfilePicUrl) &&
                            !string.IsNullOrEmpty(DataManager.Data.GetLocalProfilePicPath()))
                        {
                            OpenImageCallBack(File.ReadAllBytes(DataManager.Data.GetLocalProfilePicPath()));
                        }

                        EnableElements();
                    }
                    else
                    {
                        DisableElements();
                        UIManager.Instance.GetResponse<string>("{\"msg\": \"User is not Registered!!\"}", null, false);
                    }
                }
            }
            else
            {
                DisableElements();
            }
        }

        public void EnableElements()
        {
            m_sendRequestButton.interactable = true;
            m_cancelRequestButton.interactable = true;
            m_nicknameInput.interactable = true;
            m_passwordInput.interactable = true;
            m_profilePicOpenButton.interactable = true;
        }

        public void DisableElements()
        {
            m_sendRequestButton.interactable = false;
            m_cancelRequestButton.interactable = false;
            m_nicknameInput.interactable = false;
            m_passwordInput.interactable = false;
            m_profilePicOpenButton.interactable = false;
        }

        public void OpenImageCallBack(byte[] bytes)
        {
            Texture2D texture2d = new Texture2D(150, 150);
            texture2d.LoadImage(bytes);
            profilePicBytes = bytes;
            m_profilePicValue.sprite =
                Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), Vector2.zero);
        }

        public override void SendRequest()
        {
            base.SendRequest();
            Request requestClass = RequestManager.Instance.GetRequest(RequestNameWithApiVersion);
            requestClass.AddHeader("Host", $"x8ki-letl-twmt.n7.xano.io");
            requestClass.AddHeader("Authorization", $"Bearer {m_authenticationTokenValue.text}");
            if (!string.IsNullOrEmpty(m_passwordInput.text))
                requestClass.AddBody("password", m_passwordInput.text);
            requestClass.AddBody("nickname", m_nicknameInput.text);
            requestClass.AddBinaryData("profilepic", profilePicBytes, "profilePic.jpg");
            RequestBundle bndleReq = new RequestBundle(requestClass);
            ServerAccess.Instance.SendBatchRequest(bndleReq);
        }
    }
}