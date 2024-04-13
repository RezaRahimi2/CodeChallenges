using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectManager : MonoBehaviour
{
    [SerializeField] private List<CollectableCube> m_collectableCube;

    public Color Initialize(List<Color> collectableCubeColorSet)
    {
        m_collectableCube = FindObjectsOfType<CollectableCube>().ToList();

        Color rndColor = collectableCubeColorSet[Random.Range(0, collectableCubeColorSet.Count - 1)];
        
        m_collectableCube.ForEach(x =>
        {
            x.Initialize(this,rndColor);
        });

        return rndColor;
    }

    public void HitToCollectableCube(CollectableCube collectableCube)
    {
        GameManager.Instance.HitToCollectableCube(collectableCube);
    }
    
}
