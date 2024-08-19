#nullable enable

using UnityEngine;

namespace Zombies {
    public class ZombieMovement : MovingObject {
        private Transform? target;
        private float speed = 0f;

        public float? Axis { get; private set; }

        public void Init(Vector2 position, Transform target, float speed) {
            Teleport(position);
            this.target = target;
            this.speed = speed;
        }

        protected override Vector2 Move(float deltaTime) {
            Axis =
                this.target != null
                ? Mathf.Sign(this.target.position.x - transform.position.x)
                : null;
            return base.Move(deltaTime);
        }

        protected override Vector2 GetMovement(float deltaTime) {
            if (!Axis.HasValue
                || Mathf.Approximately(this.speed, 0f)) {
                return Vector2.zero;
            }
            return new Vector2(Axis.Value * this.speed * deltaTime, 0f);
        }
    }
}
