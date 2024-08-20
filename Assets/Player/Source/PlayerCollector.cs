#nullable enable

using Collectables;
using System;
using UnityEngine;

namespace Player {
    public class PlayerCollector : MonoBehaviour {
        public event Action<ICollectable>? Collecting;

        private void OnCollisionEnter2D(Collision2D collision) {
            Collect(collision.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            Collect(other.gameObject);
        }

        private void Collect(GameObject obj) {
            if (!obj.TryGetComponentInChildren(out ICollectable? collectable)) {
                return;
            }
            Collecting?.Invoke(collectable);
            collectable.Collect();
        }
    }
}
