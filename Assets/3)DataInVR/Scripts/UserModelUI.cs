using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Immersed.General;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Immersed.Task3
{
    public class UserModelUI : MonoBehaviour
    {
        [SerializeField] private Button m_loginButton;
        [SerializeField] private TextMeshProUGUI m_loginButtonText;

        [SerializeField] private TMP_InputField m_emailInputFiled;
        [SerializeField] private CanvasGroup m_emailInputFiledCanvasGroup;
        public string EmailInput => m_emailInputFiled.text;

        [SerializeField] private TMP_InputField m_passwordInputFiled;
        [SerializeField] private CanvasGroup m_passwordInputFiledCanvasGroup;

        [SerializeField] private CanvasGroup m_emailCanvasGroup;
        [SerializeField] private TextMeshProUGUI m_emailTextMeshValue;

        [SerializeField] private CanvasGroup m_linksCanvasGroup;
        public string PasswordInput => m_passwordInputFiled.text;

        [SerializeField] private Transform m_afterClickLoginButtonPosition;
        [SerializeField] private CanvasGroup m_loadingObjectCanvasGroup;
        [SerializeField] private Image m_profileImage;

        [SerializeField] private Transform m_linksContentParent;
        [SerializeField] private TextMeshProUGUI m_linkTextPrefab;

        public void Initialize(Action loginButtonOnClickAction)
        {
            m_loginButton.onClick.AddListener(loginButtonOnClickAction.Invoke);
            m_loginButton.onClick.AddListener(OnLoginButtonClick);
        }

        public void ShowProfileImage(Texture2D texture2d)
        {
            m_profileImage.DOColor(Color.white, 1);
            m_profileImage.sprite =
                Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), Vector2.zero);
            m_loadingObjectCanvasGroup.DOFade(0, .5f).OnComplete(() =>
            {
                m_loadingObjectCanvasGroup.gameObject.SetActive(false);
            });
        }

        public void OnLogin(bool status, UserModel userModel)
        {
            if (status)
            {
                m_loadingObjectCanvasGroup.DOFade(0, .25f).OnComplete(() =>
                {
                    m_loadingObjectCanvasGroup.gameObject.SetActive(false);
                    m_loadingObjectCanvasGroup.transform.position = m_profileImage.transform.position;
                    m_profileImage.color = new Color(.5f, .5f, .5f, 0);
                    m_loadingObjectCanvasGroup.gameObject.SetActive(true);
                    m_profileImage.gameObject.SetActive(true);
                    m_profileImage.DOFade(1, .5f);
                    m_loadingObjectCanvasGroup.DOFade(1, .5f).SetDelay(.25f);
                });

                m_loginButton.image.DOFade(0, 1);
                m_loginButtonText.text = userModel.Nickname;
                m_loginButton.transform.DOLocalMoveX(m_afterClickLoginButtonPosition.localPosition.x, 2);
                m_loginButton.transform.DOLocalMoveY(m_afterClickLoginButtonPosition.localPosition.y, 2);
                m_emailInputFiledCanvasGroup.DOFade(0, .5f);
                m_passwordInputFiledCanvasGroup.DOFade(0, .5f);

                m_emailCanvasGroup.alpha = 0;
                m_emailTextMeshValue.text = userModel.email;
                m_emailCanvasGroup.gameObject.SetActive(true);
                m_emailCanvasGroup.DOFade(1, 3).SetDelay(1);

                if (userModel.Links != null)
                {
                    foreach (string userModelLink in userModel.Links)
                    {
                        TextMeshProUGUI textMeshProUGUI =
                            Instantiate<TextMeshProUGUI>(m_linkTextPrefab, m_linksContentParent);
                        textMeshProUGUI.text = userModelLink;
                    }

                    Instantiate<TextMeshProUGUI>(m_linkTextPrefab, m_linksContentParent);
                    m_linksCanvasGroup.alpha = 0;
                    m_linksCanvasGroup.gameObject.SetActive(true);
                    m_linksCanvasGroup.DOFade(1, 3).SetDelay(1);
                }
            }
            else
            {
                m_emailInputFiled.interactable = true;
                m_passwordInputFiled.interactable = true;
                m_loginButton.interactable = true;
            }
        }

        private void OnLoginButtonClick()
        {
            m_emailInputFiled.interactable = false;
            m_passwordInputFiled.interactable = false;
            m_loginButton.interactable = false;
            DOTweenTMPAnimator animator = new DOTweenTMPAnimator(m_emailInputFiled.textComponent);
            Sequence sequence = DOTween.Sequence();
            TextAnimation(animator, sequence);
            animator = new DOTweenTMPAnimator(m_passwordInputFiled.textComponent);
            TextAnimation(animator, sequence);

            m_loadingObjectCanvasGroup.alpha = 0;
            m_loadingObjectCanvasGroup.gameObject.SetActive(true);
            m_loadingObjectCanvasGroup.DOFade(1, 1);
        }

        private static void TextAnimation(DOTweenTMPAnimator animator, Sequence sequence)
        {
            for (int i = 0; i < animator.textInfo.characterCount; ++i)
            {
                if (!animator.textInfo.characterInfo[i].isVisible) continue;
                sequence.Join(animator.DOShakeCharScale(i, .5f, 5));
            }
        }
    }
}