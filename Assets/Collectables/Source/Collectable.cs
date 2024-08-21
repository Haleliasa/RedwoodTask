#nullable enable

using System;
using System.Linq;
using UnityEngine;

namespace Collectables {
    public class Collectable : MonoBehaviour, ICollectable, ISerializedEventListener {
        [Header(EditorHeaders.References)]
        [SerializeField]
        private CollectableView? view;

        [Header(EditorHeaders.EventSubscriptions)]
        [SerializeField]
        private SerializedEvent[] destroyOnEvents = null!;

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

        public virtual void OnSerializedEvent(SerializedEvent ev, UnityEngine.Object source) {
            if (this.destroyOnEvents.Contains(ev)) {
                DestroySelf();
            }
        }

        protected virtual void OnEnable() {
            this.destroyOnEvents.ForEach(ev => ev.Subscribe(this));
        }

        protected virtual void Start() {
            UpdateView();
        }

        protected virtual void OnDisable() {
            this.destroyOnEvents.ForEach(ev => ev.Unsubscribe(this));
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
    }
}
