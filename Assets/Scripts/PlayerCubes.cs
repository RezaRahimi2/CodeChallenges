using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerCubes : MonoBehaviour
{
    private Player m_player;
    [SerializeField] private List<PlayerCube> m_cubes;
    [SerializeField] public List<PlayerCube> Cubes => m_cubes;

    public void Initialize()
    {
        m_cubes = transform.GetComponentsInChildren<PlayerCube>().ToList();
    }
    
    public void AddCube(CollectableCube collectableCube)
    {
        PlayerCube playerCube = collectableCube.gameObject.AddComponent<PlayerCube>();
        playerCube.Initialize();
        collectableCube.gameObject.tag = "PlayerCube";
        Vector3 lastCubePosition = m_cubes[0].transform.position;
        collectableCube.transform.localScale = Vector3.one;
        collectableCube.transform.position = new Vector3(lastCubePosition.x,lastCubePosition.y + playerCube.Size.y,lastCubePosition.z);
        collectableCube.transform.SetParent(transform);
        Destroy(collectableCube.GetComponent<CollectableCube>());
        m_cubes.Insert(0,playerCube);
    }

    public void RemoveCube(PlayerCube playerCube)
    {
        m_cubes.Remove(playerCube);
    }
    
    public void SortCubeByDistance()
    {
        m_cubes = m_cubes.OrderBy(cube => (m_player.transform.position - cube.transform.position).sqrMagnitude)
            .ToList();
    }
}
