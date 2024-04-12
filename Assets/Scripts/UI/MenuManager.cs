using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public CanvasGroup MainMenuPanelCanvasGroup;

    public void StartGame()
    {
        GameManager.Instance.Initialize(PlayerPrefs.GetInt("LastPlayedLevel", 0));
        MainMenuPanelCanvasGroup.DOFade(0, 1).OnComplete(() =>
        {
            MainMenuPanelCanvasGroup.gameObject.SetActive(false);
            GameManager.Instance.StartGame();
        });
    }
}