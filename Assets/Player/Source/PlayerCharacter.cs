#nullable enable

using Collectables;
using System.Collections;
using System.Linq;
using UnityEngine;
using Zombies;

namespace Player {
    public class PlayerCharacter : MonoBehaviour,
        IInjectComponent,
        IShootTiming,
        ISerializedEventListener {
        [Header(EditorHeaders.References)]
        [SerializeField]
        private PlayerMovement movement = null!;

        [SerializeField]
        private PlayerShoot shoot = null!;

        [SerializeField]
        private PlayerCollector collector = null!;

        [SerializeField]
        private PlayerView view = null!;

        [Header(EditorHeaders.EventSubscriptions)]
        [SerializeField]
        private SerializedEvent[] startOnEvents = null!;

        [SerializeField]
        private SerializedEvent[] stopOnEvents = null!;

        private bool started = false;
        private float moveAxisNonZero = 1f;

        [Inject]
        public void Inject(ZombieArmy zombieArmy) {
            zombieArmy.Init(transform);
        }

        IEnumerator IShootTiming.BeforeShoot(float interval) {
            yield return this.view.Shoot(interval);
        }

        void ISerializedEventListener.OnSerializedEvent(SerializedEvent ev, Object source) {
            if (this.startOnEvents.Contains(ev)) {
                ToggleParts(true);
                this.shoot.ResetAmmo();
                this.started = true;
            } else if (this.stopOnEvents.Contains(ev)) {
                ToggleParts(false);
                this.started = false;
            }
        }

        private void OnEnable() {
            if (this.started) {
                ToggleParts(true);
            }
            this.startOnEvents.ForEach(ev => ev.Subscribe(this));
            this.stopOnEvents.ForEach(ev => ev.Subscribe(this));
            this.collector.Collecting += OnCollecting;
        }

        private void Start() {
            this.shoot.SetTiming(this);

            // disable if not yet started
            if (!this.started) {
                ToggleParts(false);
            }
        }

        private void Update() {
            if (!Mathf.Approximately(this.movement.Axis, 0f)) {
                this.moveAxisNonZero = this.movement.Axis;
            }
            this.shoot.SetAngle(this.moveAxisNonZero > 0f ? 0f : 180f);
            this.view.SetMoveAxis(this.movement.Axis);
        }

        private void OnDisable() {
            ToggleParts(false);
            this.startOnEvents.ForEach(ev => ev.Unsubscribe(this));
            this.stopOnEvents.ForEach(ev => ev.Unsubscribe(this));
            this.collector.Collecting -= OnCollecting;
        }

        private void ToggleParts(bool enable) {
            this.movement.enabled = enable;
            this.shoot.enabled = enable;
            if (!enable) {
                this.view.SetMoveAxis(0f);
            }
        }

        private void OnCollecting(ICollectable collectable) {
            if (collectable.Type == CollectableTypes.AmmoPack) {
                this.shoot.AddAmmo(collectable.Value);
            }
        }
    }
}
