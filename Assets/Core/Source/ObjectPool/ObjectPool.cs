using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<T> : MonoBehaviour, IObjectPool<T> where T : Object {
    [SerializeField]
    private T prefab;

    [Min(0)]
    [SerializeField]
    private int startSize = 0;

    private readonly Stack<T> pool = new();

    private void Start() {
        for (int i = 0; i < this.startSize; i++) {
            this.pool.Push(Instantiate(this.prefab, transform));
        }
    }

    public T Get() {
        if (this.pool.TryPop(out T obj)) {
            return obj;
        }
        return Instantiate(this.prefab, transform);
    }

    public void Return(T obj) {
        this.pool.Push(obj);
    }
}
