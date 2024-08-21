#nullable enable

using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class PlayerMovement : MovingObject {
        [Header(nameof(PlayerMovement))]
        [Header(EditorHeaders.References)]
        [SerializeField]
        private InputActionAsset inputActions = null!;

        [Header(EditorHeaders.Properties)]
        [Tooltip("units/sec")]
        [Min(1f)]
        [SerializeField]
        private float speed = 10f;

        private CharacterActions actions;

        public float Axis { get; private set; }

        protected override Vector2 Move(float deltaTime) {
            Axis = this.actions.Move.ReadValue<float>();
            return base.Move(deltaTime);
        }

        protected override Vector2 GetMovement(float deltaTime) {
            return new Vector2(Axis * this.speed * deltaTime, 0f);
        }

        private void Awake() {
            this.actions = new CharacterActions(this.inputActions);
        }

        private void OnDisable() {
            Axis = 0f;
        }
    }
}
