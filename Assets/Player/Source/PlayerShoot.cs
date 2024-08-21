#nullable enable

using Projectiles;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class PlayerShoot : MonoBehaviour, IInjectComponent {
        [Header(EditorHeaders.References)]
        [SerializeField]
        private InputActionAsset inputActions = null!;

        [SerializeField]
        private Transform? projectilePosition;

        [Header(EditorHeaders.Events)]
        [SerializeField]
        private SerializedEvent<int>[] ammoChangedEvents = null!;

        [Header(EditorHeaders.Properties)]
        [Min(0)]
        [SerializeField]
        private int startAmmo = 100;

        [Tooltip("sec")]
        [Min(0f)]
        [SerializeField]
        private float interval = 0.5f;

        [SerializeField]
        private bool overrideProjectileProperties = false;

        [SerializeField]
        private ProjectileProperties projectileProperties;

        private CharacterActions actions;
        private IObjectPool<Projectile> projectilePool = null!;
        private int ammo;
        private Coroutine? shootCoroutine;
        private IShootTiming? timing;
        private float angle = 0f;

        private Vector2 ProjectilePos =>
            this.projectilePosition != null
            ? this.projectilePosition.position
            : transform.position;

        private ProjectileProperties? ProjectileProps =>
            this.overrideProjectileProperties
            ? this.projectileProperties
            : null;

        [Inject]
        public void Inject(
            // key in case of multiple pools of type Projectile
            [InjectKey(InjectKeys.PlayerProjectilePool)] IObjectPool<Projectile> projectilePool) {
            this.projectilePool = projectilePool;
        }

        public void SetTiming(IShootTiming? timing) {
            this.timing = timing;
        }

        public void SetAngle(float angleDeg) {
            this.angle = angleDeg;
        }

        public void AddAmmo(int amount) {
            if (amount > 0) {
                ChangeAmmo(amount);
            }
        }

        public void ResetAmmo() {
            SetAmmo(this.startAmmo);
        }

        private void Awake() {
            this.actions = new CharacterActions(this.inputActions);
        }

        private void OnEnable() {
            this.actions.Shoot.started += StartShooting;
        }

        private void Start() {
            ResetAmmo();
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
                StartCoroutine(Shoot());
                yield return new WaitForSeconds(this.interval);
            } while (this.actions.Shoot.IsInProgress());
            this.shootCoroutine = null;
        }

        private IEnumerator Shoot() {
            if (this.ammo == 0)
            {
                yield break;
            }
            if (this.timing != null) {
                yield return this.timing.BeforeShoot(this.interval);
            }
            Projectile.Launch(
                this.projectilePool,
                ProjectilePos,
                this.angle,
                properties: ProjectileProps);
            ChangeAmmo(-1);
        }

        private void ChangeAmmo(int delta) {
            SetAmmo(this.ammo + delta);
        }

        private void SetAmmo(int amount) {
            this.ammo = Mathf.Max(amount, 0);
            this.ammoChangedEvents.ForEach(e => e.InvokeTyped(this, this.ammo));
        }
    }
}
