using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    public abstract void Initialize();
}

public abstract class Controller<T> : MonoBehaviour
{
    public abstract void Initialize(T t);
}

