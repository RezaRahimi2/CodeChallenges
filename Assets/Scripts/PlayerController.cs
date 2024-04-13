using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Player m_player;
    public Player Player => m_player;
    public Transform PlayerTransform => m_player.transform;


    public void Initialize(Color trialColor)
    {
        m_player.Initialize(trialColor);
    }

    public void SetPlayerPosition(Vector3 newPos)
    {
        m_player.SetPosition(newPos);
    }

    public void AddCubeToPlayer(CollectableCube collectableCube)
    {
        m_player.AddCubeToPlayer(collectableCube);
    }

    public void RemoveCubeFromPlayer(PlayerCube playerCube)
    {
        m_player.RemoveCubeFromPlayer(playerCube);
    }

    public void GameIsOver(bool playerIsWinner)
    {
        m_player.GameIsOver(playerIsWinner);
    }
}
