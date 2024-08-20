using System;
using UnityEngine;

namespace Projectiles {
    [Serializable]
    public struct ProjectileProperties {
        [Tooltip("units")]
        [Min(0f)]
        [SerializeField]
        private float distance;

        [Tooltip("sec")]
        [Min(0f)]
        [SerializeField]
        private float duration;

        [Min(0f)]
        [SerializeField]
        private float damage;

        public float Distance {
            readonly get => this.distance;
            set => this.distance = Mathf.Max(value, 0f);
        }

        public float Duration {
            readonly get => this.duration;
            set => this.duration = Mathf.Max(value, 0f);
        }

        public float Damage {
            readonly get => this.damage;
            set => this.damage = Mathf.Max(value, 0f);
        }
    }
}
