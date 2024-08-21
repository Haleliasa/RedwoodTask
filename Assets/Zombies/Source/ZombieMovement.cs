#nullable enable

using UnityEngine;

namespace Zombies {
    public class ZombieMovement : MovingObject {
        [Header(nameof(ZombieMovement))]
        [Header(EditorHeaders.References)]
        [SerializeField]
        private Transform? target;

        [Header(EditorHeaders.Properties)]
        [SerializeField]
        private float speed = 0f;

        public float Axis { get; private set; }

        public void Init(Vector2 position, Transform target, float speed) {
            Teleport(position);
            this.target = target;
            this.speed = speed;
        }

        protected override Vector2 Move(float deltaTime) {
            Axis =
                this.target != null
                ? Mathf.Sign(this.target.position.x - transform.position.x)
                : 0f;
            return base.Move(deltaTime);
        }

        protected override Vector2 GetMovement(float deltaTime) {
            return new Vector2(Axis * this.speed * deltaTime, 0f);
        }

        private void OnDisable() {
            Axis = 0f;
        }
    }
}
