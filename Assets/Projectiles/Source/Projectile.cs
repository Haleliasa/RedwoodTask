#nullable enable

using UnityEngine;

namespace Projectiles {
    public class Projectile : MovingObject {
        [Header(nameof(Projectile))]
        [SerializeField]
        private float distance = 20f;

        [SerializeField]
        private float duration = 1f;

        [SerializeField]
        private float damage = 10f;

        private Vector2 dir;
        private float speed;
        private float? timeLeft;

        public static Projectile Launch(Projectile prefab, Vector2 from, float angleDeg) {
            Projectile projectile = Instantiate(prefab);
            projectile.Launch(from, angleDeg);
            return projectile;
        }

        public static Projectile Launch(IObjectPool<Projectile> pool, Vector2 from, float angleDeg) {
            Projectile projectile = pool.Get();
            projectile.Launch(from, angleDeg);
            return projectile;
        }

        public void Launch(Vector2 from, float angleDeg) {
            if (Mathf.Approximately(this.distance, 0f)
                || Mathf.Approximately(this.duration, 0f)) {
                Destroy(gameObject);
                return;
            }

            transform.SetPositionAndRotation(from, Quaternion.Euler(0f, 0f, angleDeg));
            this.dir = Vector2.right.Rotate(angleDeg);
            this.speed = this.distance / this.duration;
            this.timeLeft = this.duration;
        }

        protected override void Update() {
            if (this.timeLeft.HasValue) {
                this.timeLeft -= Time.deltaTime;
                if (this.timeLeft <= 0f) {
                    Destroy(gameObject);
                    this.timeLeft = null;
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

        private void OnCollisionEnter2D(Collision2D collision) {
            Hit(collision.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            Hit(collision.gameObject);
        }

        private void Hit(GameObject obj) {
            if (obj.TryGetComponentInChildren(out IHittable? hittable))
            {
                hittable.Hit(this.damage);
            }
            Destroy(gameObject);
        }
    }
}
