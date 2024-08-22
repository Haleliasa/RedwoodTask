#nullable enable

using System;
using UnityEngine;

namespace Projectiles {
    public class Projectile : MovingObject {
        [Header(nameof(Projectile))]
        [SerializeField]
        private ProjectileProperties properties;

        private ProjectileProperties props;
        private IDisposable? pooled;
        private Vector2 dir;
        private float speed;
        private float? timeLeft;

        public ProjectileProperties Properties => this.properties;

        public static T Launch<T>(
            T prefab,
            Vector2 from,
            float angleDeg,
            ProjectileProperties? properties = null)
            where T : Projectile {
            T projectile = Instantiate(prefab);
            projectile.Launch(from, angleDeg, properties);
            return projectile;
        }

        public static T Launch<T>(
            IObjectPool<T> pool,
            Vector2 from,
            float angleDeg,
            ProjectileProperties? properties = null)
            where T : Projectile {
            IDisposableObject<T> pooled = pool.Get();
            T projectile = pooled.Object;
            projectile.pooled = pooled;
            projectile.Launch(from, angleDeg, properties);
            return projectile;
        }

        public void Launch(
            Vector2 from,
            float angleDeg,
            ProjectileProperties? properties = null) {
            this.props = properties ?? this.properties;

            if (Mathf.Approximately(this.props.Distance, 0f)
                || Mathf.Approximately(this.props.Duration, 0f)) {
                DestroySelf();
                return;
            }

            transform.SetPositionAndRotation(from, Quaternion.Euler(0f, 0f, angleDeg));
            this.dir = Vector2.right.Rotate(angleDeg);
            this.speed = this.props.Distance / this.props.Duration;
            this.timeLeft = this.props.Duration;
        }

        protected override void Update() {
            if (this.timeLeft.HasValue) {
                this.timeLeft -= Time.deltaTime;
                if (this.timeLeft <= 0f) {
                    this.timeLeft = null;
                    DestroySelf();
                }
            }
            base.Update();
        }

        protected override Vector2 GetMovement(float deltaTime) {
            if (!this.timeLeft.HasValue) {
                return Vector2.zero;
            }
            return this.dir * (this.speed * deltaTime);
        }

        protected virtual void Hit(GameObject obj) {
            if (obj.TryGetComponentInChildren(out IHittable? hittable)) {
                hittable.Hit(this.props.Damage);
            }
            DestroySelf();
        }

        protected void DestroySelf() {
            if (this.pooled != null) {
                this.pooled.Dispose();
            } else {
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            Hit(collision.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            Hit(collision.gameObject);
        }
    }
}
