using System.Collections.Generic;
using System.Linq;
using Immersed.General;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Immersed.Task1
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private List<ButtonWithRequestName> m_buttonsWithRequestNames;
        [SerializeField] private List<PanelWithRequestName> m_panelsWithNames;
        [SerializeField] private TextMeshProUGUI m_resultText;
        [SerializeField] private GameObject m_loadingGameObject;
        [SerializeField] private PanelWithRequestName m_lastPanel;
        private EventSystem system;

        public void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            system = EventSystem.current;

            RequestManager.Instance.OnResponseUserEvent.OnResponse += GetResponse;

            m_buttonsWithRequestNames = FindObjectsOfType<ButtonWithRequestName>().ToList();

            m_panelsWithNames = FindObjectsOfType<PanelWithRequestName>(true).ToList();
            m_panelsWithNames.ForEach(x => { x.Initialize(); });


            m_buttonsWithRequestNames.ForEach(x => { x.Initialize(() => ShowPanel(x.RequestNameWithApiVersion)); });

            HideAll(true);
        }

        public void ShowPanel(RequestNameWithApiVersion requestNameWithApiVersion)
        {
            HideAll(apiVersion: requestNameWithApiVersion.APIVersion,
                requestName: requestNameWithApiVersion.RequestName);
            m_lastPanel = m_panelsWithNames.Find(x =>
                x.RequestNameWithApiVersion.RequestName == requestNameWithApiVersion.RequestName &&
                x.RequestNameWithApiVersion.APIVersion == requestNameWithApiVersion.APIVersion);
            m_lastPanel.Show();
        }

        public void ShowLoading()
        {
            m_loadingGameObject.SetActive(true);
        }

        public void HideLoading()
        {
            m_loadingGameObject.SetActive(false);
        }

        public void HideAll(bool withoutTween = false, APIVersion apiVersion = APIVersion.None,
            RequestName requestName = RequestName.None)
        {
            m_panelsWithNames.ForEach(x =>
            {
                if (x.RequestNameWithApiVersion.APIVersion != apiVersion &&
                    x.RequestNameWithApiVersion.RequestName != requestName)
                    x.Hide(withoutTween);
            });
        }

        public void GetResponse<T>(string dataString, T responseData, bool isSuccess)
        {
            if (isSuccess)
            {
                m_lastPanel.OnResponse<T>(responseData);
                m_resultText.color = Color.green;
            }
            else
                m_resultText.color = Color.red;

            HideLoading();

            if (JsonExtensions.IsValidJson(dataString))
                m_resultText.text = dataString?.FormatJson();
            else
                m_resultText.text = dataString;
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>()
                        .FindSelectableOnDown();

                    if (next != null)
                    {
                        InputField inputfield = next.GetComponent<InputField>();
                        if (inputfield != null)
                            inputfield.OnPointerClick(
                                new PointerEventData(system)); //if it's an input field, also set the text caret

                        system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
                    }

                    //else Debug.Log("next nagivation element not found");
                }
            }
        }
    }
}