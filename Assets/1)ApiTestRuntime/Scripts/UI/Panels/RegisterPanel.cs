using Immersed.General;
using TMPro;
using UnityEngine;

namespace Immersed.Task1
{
    public class RegisterPanel : PanelWithRequestName
    {
        [SerializeField] private TextMeshProUGUI m_deviceID;
        [SerializeField] private TMP_InputField m_emailInput;
        [SerializeField] private TMP_InputField m_passwordInput;
        [SerializeField] private TMP_InputField m_nicknameInput;

        public override void Show()
        {
            base.Show();
            m_deviceID.text = SystemInfo.deviceUniqueIdentifier;
        }

        public override void SendRequest()
        {
            base.SendRequest();
            Request requestClass = RequestManager.Instance.GetRequest(RequestNameWithApiVersion);
            requestClass.AddBody("email", m_emailInput.text);
            requestClass.AddBody("password", m_passwordInput.text);
            requestClass.AddBody("nickname", m_nicknameInput.text);
            requestClass.AddBody("deviceid", m_deviceID.text);
            RequestBundle bndleReq = new RequestBundle(requestClass);
            ServerAccess.Instance.SendBatchRequest(bndleReq);
        }
    }
}