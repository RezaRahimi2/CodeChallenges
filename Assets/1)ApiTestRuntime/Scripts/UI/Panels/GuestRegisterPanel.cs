using Immersed.General;
using TMPro;
using UnityEngine;

namespace Immersed.Task1
{
    public class GuestRegisterPanel : PanelWithRequestName
    {
        [SerializeField] TextMeshProUGUI m_deviceID;

        public override void Show()
        {
            base.Show();
            m_deviceID.text = SystemInfo.deviceUniqueIdentifier;
        }

        public override void SendRequest()
        {
            base.SendRequest();

            Request requestClass = RequestManager.Instance.GetRequest(RequestNameWithApiVersion);
            requestClass.AddBody("deviceid", m_deviceID.text);
            RequestBundle bndleReq = new RequestBundle(requestClass);
            ServerAccess.Instance.SendBatchRequest(bndleReq);
        }
    }
}