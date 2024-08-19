#nullable enable

using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class PlayerMovement : MovingObject {
        [Header(nameof(PlayerMovement))]
        [SerializeField]
        private InputActionAsset inputActions = null!;

        [Tooltip("units/sec")]
        [Min(1f)]
        [SerializeField]
        private float speed = 10f;

        private CharacterActions actions;

        public float Axis { get; private set; }

        private void Awake() {
            this.actions = new CharacterActions(this.inputActions);
        }

        protected override Vector2 Move(float deltaTime) {
            Axis = this.actions.Move.ReadValue<float>();
            return base.Move(deltaTime);
        }

        protected override Vector2 GetMovement(float deltaTime) {
            return new Vector2(Axis * this.speed * deltaTime, 0f);
        }
    }
}
