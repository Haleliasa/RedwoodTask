#nullable enable

using Collectables;
using System;
using UnityEngine;

namespace Zombies {
    public class Zombie : MonoBehaviour {
        [SerializeField]
        private ZombieStats stats = null!;

        [SerializeField]
        private ZombieMovement movement = null!;

        [SerializeField]
        private ZombieView view = null!;

        [SerializeField]
        private Collectable resourcePrefab = null!;

        [SerializeField]
        private Transform? resourcePosition;

        private ZombieType? type;

        private Vector2 ResourcePos =>
            this.resourcePosition != null
            ? this.resourcePosition.position
            : transform.position;

        public event Action<Zombie>? Died;

        private void OnEnable() {
            this.stats.TookDamage += OnTookDamage;
            this.stats.Died += OnDied;
        }

        private void OnDisable() {
            this.stats.TookDamage -= OnTookDamage;
            this.stats.Died -= OnDied;
        }

        public void Init(ZombieType type, Vector2 position, Transform target) {
            this.type = type;
            this.stats.Init(type.Health);
            this.movement.Init(position, target, type.Speed);
            this.view.Init(type.ViewType);
        }

        private void Update() {
            if (this.movement.Axis.HasValue) {
                this.view.SetFlip(this.movement.Axis.Value < 0f);
            }
        }

        private void OnTookDamage(float damage) {
            this.view.SetHealth(
                this.stats.MaxHealth != 0f
                ? (this.stats.Health / this.stats.MaxHealth)
                : 0f);
        }

        private void OnDied() {
            int resources =
                this.type != null
                ? UnityEngine.Random.Range(this.type.MinResources, this.type.MaxResources + 1)
                : 0;
            if (resources > 0) {
                Collectable.Place(this.resourcePrefab, ResourcePos, value: resources);
            }
            Died?.Invoke(this);
        }
    }
}
