using Immersed.General;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

namespace Immersed.Task1
{
    public class UserDataPanel : PanelWithRequestName
    {
        [SerializeField] private TextMeshProUGUI authenticationTokenValue;
        [SerializeField] private TMP_InputField m_passwordInput;
        [SerializeField] private TMP_InputField m_nicknameInput;
        [SerializeField] private TextMeshProUGUI emailValue;

        public override void Show()
        {
            base.Show();
            if (PlayerPrefs.HasKey(DataManager.Data.PREFS_SECRET_KEY))
            {
                string encrypted = PlayerPrefs.GetString(DataManager.Data.PREFS_SECRET_KEY);
                authenticationTokenValue.text = AES.Decrypt(encrypted);
                if (PlayerPrefs.HasKey(DataManager.Data.PREFS_PROFILE_DATA))
                {
                    encrypted = PlayerPrefs.GetString(DataManager.Data.PREFS_PROFILE_DATA);
                    UserModel userModelData = JsonConvert.DeserializeObject<UserModel>(AES.Decrypt(encrypted, "ud"));
                    m_nicknameInput.text = userModelData.Nickname;
                    emailValue.text = userModelData.email;

                }

                EnableElements();
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
        }

        public void DisableElements()
        {
            m_sendRequestButton.interactable = false;
            m_cancelRequestButton.interactable = false;
            m_nicknameInput.interactable = false;
            m_passwordInput.interactable = false;
        }

        public override async void SendRequest()
        {
            base.SendRequest();
            Request requestClass = RequestManager.Instance.GetRequest(RequestNameWithApiVersion);
            requestClass.AddHeader("Host", $"x8ki-letl-twmt.n7.xano.io");
            requestClass.AddHeader("Authorization", $"Bearer {authenticationTokenValue.text}");
            RequestBundle bndleReq = new RequestBundle(requestClass);
            ServerAccess.Instance.SendBatchRequest(bndleReq);
        }
    }
}