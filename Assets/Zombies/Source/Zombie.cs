#nullable enable

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
            this.stats.Init(
                type.Health,
                UnityEngine.Random.Range(type.MinResources, type.MaxResources + 1));
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
            Died?.Invoke(this);
        }
    }
}
