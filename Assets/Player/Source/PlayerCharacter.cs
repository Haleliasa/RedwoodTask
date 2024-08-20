#nullable enable

using Collectables;
using System.Collections;
using UnityEngine;
using Zombies;

namespace Player {
    public class PlayerCharacter : MonoBehaviour, IInjectComponent, IShootTiming {
        [SerializeField]
        private PlayerMovement movement = null!;

        [SerializeField]
        private PlayerShoot shoot = null!;

        [SerializeField]
        private PlayerCollector collector = null!;

        [SerializeField]
        private PlayerView view = null!;

        private float moveAxisNonZero = 1f;

        [Inject]
        public void Inject(ZombieArmy zombieArmy) {
            zombieArmy.Init(transform);
        }

        IEnumerator IShootTiming.BeforeShoot(float interval) {
            yield return this.view.Shoot(interval);
        }

        private void Start() {
            this.shoot.SetTiming(this);
        }

        private void OnEnable() {
            this.collector.Collecting += OnCollecting;
        }

        private void OnDisable() {
            this.collector.Collecting -= OnCollecting;
        }

        private void Update() {
            if (!Mathf.Approximately(this.movement.Axis, 0f)) {
                this.moveAxisNonZero = this.movement.Axis;
            }
            this.shoot.SetAngle(this.moveAxisNonZero > 0f ? 0f : 180f);
            this.view.SetMoveAxis(this.movement.Axis);
        }

        private void OnCollecting(ICollectable collectable) {
            if (collectable.Type == CollectableTypes.AmmoPack) {
                this.shoot.AddAmmo(collectable.Value);
            }
        }
    }
}
