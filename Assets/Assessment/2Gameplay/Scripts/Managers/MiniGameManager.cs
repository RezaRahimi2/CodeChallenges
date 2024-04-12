using Promises;
using Server.API;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    private GameplayApi m_gameplayApi;

    [SerializeField] private MiniGameUIManager m_MiniGameUIManager;


    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        m_gameplayApi = new GameplayApi();
        m_MiniGameUIManager.ShowWaitingForServerAnimation();
        m_gameplayApi.Initialise().Then(() =>
            {
               m_gameplayApi.GetPlayerBalance().Then((playerBalance =>
                    {
                        Debug.Log($"<color=#00FF00>Player Balance is {playerBalance}</color>");
                        m_MiniGameUIManager.Initialize(MiniGameName.LuckyWheel, playerBalance, (updatePlayerBalance) =>
                        {
                            m_gameplayApi.SetPlayerBalance(updatePlayerBalance).Then(() =>
                            {
                                Debug.Log($"<color=#00FF00>player balance Updated {updatePlayerBalance}</color>");
                            }).Catch((e) =>
                            {
                                PlayerPrefs.SetString("LastPlayerBalance",updatePlayerBalance.ToString());
                                Debug.Log("<color=#00FF00>Failed to Update player balance</color>");
                            });
                        });
                        m_MiniGameUIManager.MiniGameShow();
                    }))
                    .Catch((e) => { Debug.Log("<color=#FF0000>Failed to get player balance</color>"); });
            })
            .Catch((e) =>
            {
                Debug.Log("<color=#FF0000>Failed to connect to server</color>");
            });
    }
}