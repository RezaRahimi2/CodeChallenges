using UnityEngine;

public abstract class Manager : MonoBehaviour
{
    public abstract void Initialize();
}

public abstract class Manager<T> : MonoBehaviour
{
    public abstract void Initialize(T t);
}

public abstract class Manager<T,T1> : MonoBehaviour
{
    public abstract void Initialize(T t,T1 t1);
}
