using Immersed.General;
using TMPro;
using UnityEngine;

namespace Immersed.Task1
{
    public class GetPlayerDataWithAuthToken : PanelWithRequestName
    {
        [SerializeField] private TMP_InputField authenticationTokenValue;

        public override void Show()
        {
            base.Show();
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