#nullable enable

using Projectiles;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class PlayerShoot : MonoBehaviour {
        [SerializeField]
        private InputActionAsset inputActions = null!;

        [SerializeField]
        private Transform? projectilePosition;

        [SerializeField]
        private Projectile projectilePrefab = null!;

        [Tooltip("sec")]
        [Min(0f)]
        [SerializeField]
        private float interval = 0.25f;

        private CharacterActions actions;
        private Coroutine? shootCoroutine;
        private float angle = 0f;

        private Vector2 ProjectilePos =>
            this.projectilePosition != null
            ? this.projectilePosition.position
            : transform.position;

        public event Action? Ready;

        public void SetAngle(float angleDeg) {
            this.angle = angleDeg;
        }

        public void Shoot() {
            Projectile.Launch(this.projectilePrefab, ProjectilePos, this.angle);
        }

        private void Awake() {
            this.actions = new CharacterActions(this.inputActions);
        }

        private void OnEnable() {
            this.actions.Shoot.started += StartShooting;
        }

        private void OnDisable() {
            this.actions.Shoot.started -= StartShooting;
            if (this.shootCoroutine != null) {
                StopCoroutine(this.shootCoroutine);
                this.shootCoroutine = null;
            }
        }

        private void StartShooting(InputAction.CallbackContext obj) {
            if (this.shootCoroutine != null) {
                return;
            }
            this.shootCoroutine = StartCoroutine(ShootRoutine());
        }

        private IEnumerator ShootRoutine() {
            do {
                Ready?.Invoke();
                yield return new WaitForSeconds(this.interval);
            } while (this.actions.Shoot.IsInProgress());
            this.shootCoroutine = null;
        }
    }
}
