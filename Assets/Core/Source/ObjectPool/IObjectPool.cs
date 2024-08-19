using UnityEngine;

public interface IObjectPool<T> where T : Object
{
    T Get();

    void Return(T obj);
}
