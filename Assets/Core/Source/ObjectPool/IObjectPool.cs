using UnityEngine;

public interface IObjectPool<T> where T : Object
{
    T Get();

    IDisposableObject<T> GetDisposable();

    void Return(T obj);
}
