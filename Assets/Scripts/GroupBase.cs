using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Group : MonoBehaviour
{
}

public abstract class GroupBase<T1, T2> : Group where T2 : CubeBase
{
    protected T1 m_manager;
    public List<T2> m_cubes;
    
    public void Initialize(T1 manager,List<Color> colorSet)
    {
        m_manager = manager;
        m_cubes = GetComponentsInChildren<T2>().ToList();
        Initialize(colorSet[Random.Range(0,colorSet.Count - 1)]);
    }

    public abstract void Initialize(Color color);

}