#nullable enable

using System;
using UnityEngine;

namespace Collectables {
    public class Collectable : MonoBehaviour, ICollectable {
        [Header(EditorHeaders.References)]
        [SerializeField]
        private CollectableView? view;

        // should use some kind of KeyList asset and SelectFrom attribute
        [Header(EditorHeaders.Properties)]
        [SerializeField]
        protected string type = null!;

        [Min(1)]
        [SerializeField]
        protected int value = 1;

        private IDisposable? pooled;

        public virtual string Name => name;

        public virtual string Type => this.type;

        public virtual int Value => this.value;

        public static T Place<T>(
            T prefab,
            Vector2 position,
            int? value = null)
            where T : Collectable {
            T collectable = Instantiate(prefab);
            collectable.Place(position, value);
            return collectable;
        }

        public static T Place<T>(
            IObjectPool<T> pool,
            Vector2 position,
            int? value = null)
            where T : Collectable {
            IDisposableObject<T> pooled = pool.GetDisposable();
            T collectable = pooled.Object;
            collectable.pooled = pooled;
            collectable.Place(position, value);
            return collectable;
        }

        public void Place(Vector2 position, int? value = null) {
            transform.position = position;
            if (value.HasValue) {
                this.value = Mathf.Max(value.Value, 1);
            }
            UpdateView();
        }

        public virtual void Collect() {
            DestroySelf();
        }

        protected void UpdateView() {
            if (this.view == null) {
                return;
            }
            this.view.SetValue(this.value);
        }

        protected void DestroySelf() {
            if (this.pooled != null) {
                this.pooled.Dispose();
            } else {
                Destroy(gameObject);
            }
        }

        private void Start() {
            UpdateView();
        }
    }
}
