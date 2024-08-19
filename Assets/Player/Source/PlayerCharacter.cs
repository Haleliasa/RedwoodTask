#nullable enable

using UnityEngine;

namespace Player {
    public class PlayerCharacter : MonoBehaviour {
        [SerializeField]
        private PlayerMovement movement = null!;

        [SerializeField]
        private PlayerShoot shoot = null!;

        [SerializeField]
        private PlayerView view = null!;

        private float moveAxisNonZero = 1f;

        private void OnEnable() {
            this.shoot.Ready += OnShootReady;
        }

        private void OnDisable() {
            this.shoot.Ready -= OnShootReady;
        }

        private void Update() {
            if (!Mathf.Approximately(this.movement.Axis, 0f)) {
                this.moveAxisNonZero = this.movement.Axis;
            }
            this.shoot.SetAngle(this.moveAxisNonZero > 0f ? 0f : 180f);
            this.view.SetMoveAxis(this.moveAxisNonZero);
        }

        private void OnShootReady() {
            // Shoot will be called on the view animator event
            //this.view.TriggerShoot();
            this.shoot.Shoot();
        }
    }
}
