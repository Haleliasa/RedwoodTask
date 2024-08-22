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
        private ProgressBar healthBar = null!;

        [Header(EditorHeaders.Properties)]
        [SerializeField]
        private ZombieViewType type = ZombieViewType.Regular;

        private bool flip = false;

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
            this.healthBar.SetProgress(percent);
        }

        private void OnEnable() {
            this.animator.enabled = true;
        }

        private void OnDisable() {
            this.animator.enabled = false;
        }
    }
}
