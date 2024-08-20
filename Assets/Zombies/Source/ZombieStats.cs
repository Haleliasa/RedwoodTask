#nullable enable

using System;
using UnityEngine;

namespace Zombies {
    public class ZombieStats : MonoBehaviour, IHittable {
        [SerializeField]
        private float maxHealth = 100f;

        public float MaxHealth => this.maxHealth;

        public float Health { get; private set; }

        public event Action<float>? TookDamage;

        public event Action? Died;

        public void Init(float maxHealth) {
            Health = this.maxHealth = maxHealth;
        }

        public void Hit(float damage) {
            damage = Mathf.Min(damage, Health);
            Health -= damage;
            TookDamage?.Invoke(damage);
            if (Mathf.Approximately(Health, 0f)) {
                Die();
            }
        }

        private void Die() {
            Died?.Invoke();
        }
    }
}
