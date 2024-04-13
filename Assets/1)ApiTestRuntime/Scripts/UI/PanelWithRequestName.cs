using System;
using System.IO;
using DG.Tweening;
using Immersed.General;
using SFB;
using UnityEngine;
using UnityEngine.UI;

namespace Immersed.Task1
{
    public abstract class PanelWithRequestName : MonoBehaviour, IRequestNameWithApiVersion
    {
        [SerializeField] public CanvasGroup CanvasGroup;
        [field: SerializeField] public RequestNameWithApiVersion RequestNameWithApiVersion { get; set; }
        [SerializeField] protected Button m_sendRequestButton;
        [SerializeField] protected Button m_cancelRequestButton;

        private Action<byte[]> m_openImageCallback;

        public virtual void Initialize(Action<byte[]> openImageCallback = null)
        {
            m_openImageCallback = openImageCallback;
            InitializeUI();
        }

        public virtual void Initialize()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
            m_sendRequestButton = transform.Find("SendRequestButton").GetComponent<Button>();
            m_cancelRequestButton = transform.Find("CancelRequestButton").GetComponent<Button>();

            m_sendRequestButton.onClick.AddListener(SendRequest);
        }

        public virtual void Show()
        {
            CanvasGroup.alpha = 0;
            gameObject.SetActive(true);
            CanvasGroup.DOFade(1, .5f);
        }

        public void Hide(bool withoutTween)
        {
            CanvasGroup.DOFade(0, withoutTween ? 0 : .5f)
                .OnComplete(() => { gameObject.SetActive(false); });
        }


        public virtual void SendRequest()
        {
            UIManager.Instance.ShowLoading();
        }

        public virtual void OnResponse<T>(T responseData)
        {
        }

        public virtual void CancelRequest()
        {
        }

        public void OpenFile()
        {
            // Open file with filter
            var extensions = new[]
            {
                new ExtensionFilter("Image Files", "png", "jpg", "jpeg"),
            };
            StandaloneFileBrowser.OpenFilePanelAsync("Open File", "", extensions, true, async (filesPath) =>
            {
                if (filesPath != null && filesPath.Length > 0)
                    using (FileStream file = File.OpenRead(filesPath[0]))
                    {
                        byte[] profilePicBytes = new byte[file.Length];
                        await file.ReadAsync(profilePicBytes, 0, (int) profilePicBytes.Length);
                        m_openImageCallback?.Invoke(profilePicBytes);
                    }
            });
        }
    }
}