using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<T> : MonoBehaviour, IConcreteObjectPool<T> where T : Object {
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
            T obj = Instantiate(this.prefab, transform);
            Return(obj);
        }
    }

    public IDisposableObject<T> Get() {
        T obj = GetConcrete();
        return new Disposable(obj, this);
    }

    public T GetConcrete() {
        if (!this.pool.TryPop(out T obj)) {
            return Instantiate(this.prefab, transform);
        }
        ToggleObject(obj, true);
        return obj;
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
