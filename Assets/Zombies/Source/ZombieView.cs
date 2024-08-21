#nullable enable

using UnityEngine;

namespace Zombies {
    public class ZombieView : MonoBehaviour {
        [Header(EditorHeaders.References)]
        [SerializeField]
        private Animator animator = null!;

        [SerializeField]
        private SpriteRenderer sprite = null!;

        [SerializeField]
        private SpriteRenderer healthBar = null!;

        [Header(EditorHeaders.Properties)]
        [SerializeField]
        private ZombieViewType type = ZombieViewType.Regular;

        private float? healthBarSize;
        private bool flip = false;

        private float HealthBarSize => this.healthBarSize ??= this.healthBar.size.x;

        public void Init(ZombieViewType type) {
            this.type = type;
            this.animator.SetInteger(ZombieAnimatorParameters.Type, (int)type);
            SetHealth(1f);
        }

        public void SetMoveAxis(float axis) {
            if (Mathf.Approximately(axis, 0f)) {
                return;
            }

            bool flip = axis < 0f;

            if (flip == this.flip) {
                return;
            }

            this.sprite.flipX ^= true;
            this.flip = flip;
        }

        public void SetHealth(float percent) {
            this.healthBar.size = this.healthBar.size.Set(x: HealthBarSize * percent);
        }

        private void OnEnable() {
            this.animator.enabled = true;
        }

        private void OnDisable() {
            this.animator.enabled = false;
        }
    }
}
