#nullable enable

using System;
using UnityEngine;

namespace Zombies {
    public class ZombieStats : MonoBehaviour, IHittable {
        private int resources;

        public float MaxHealth { get; private set; }

        public float Health { get; private set; }

        public event Action<float>? TookDamage;

        public event Action? Died;

        public void Init(float maxHealth, int resources) {
            Health = MaxHealth = maxHealth;
            this.resources = resources;
        }

        public void Hit(float damage) {
            damage = Mathf.Min(damage, Health);
            Health -= damage;
            TookDamage?.Invoke(damage);
            if (Mathf.Approximately(Health, 0f)) {
                Died?.Invoke();
            }
        }
    }
}
