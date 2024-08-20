using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<T> : MonoBehaviour, IObjectPool<T> where T : Object {
    [Header(EditorHeaders.References)]
    [SerializeField]
    private T prefab;

    [Header(EditorHeaders.Properties)]
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
            ToggleObject(obj, true);
            return obj;
        }
        return Instantiate(this.prefab, transform);
    }

    public IDisposableObject<T> GetDisposable() {
        T obj = Get();
        return new Disposable(obj, this);
    }

    public void Return(T obj) {
        ToggleObject(obj, false);
        this.pool.Push(obj);
    }

    private void ToggleObject(T obj, bool active) {
        switch (obj) {
            case GameObject gameObj:
                gameObj.SetActive(active);
                break;

            case Component component:
                component.gameObject.SetActive(active);
                break;
        }
    }

    private class Disposable : IDisposableObject<T> {
        private readonly ObjectPool<T> pool;

        public Disposable(T obj, ObjectPool<T> pool) {
            Object = obj;
            this.pool = pool;
        }

        public T Object { get; }

        public void Dispose() {
            this.pool.Return(Object);
        }
    }
}
