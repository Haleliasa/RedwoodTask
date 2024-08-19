#nullable enable

using UnityEngine;

namespace Zombies {
    public class ZombieView : MonoBehaviour {
        public const string TypeParameter = "Type";

        [SerializeField]
        private Animator animator = null!;

        [SerializeField]
        private SpriteRenderer sprite = null!;

        [SerializeField]
        private SpriteRenderer healthBar = null!;

        private float? healthBarSize;

        private float HealthBarSize => this.healthBarSize ??= this.healthBar.size.x;

        public void Init(ZombieViewType type) {
            this.animator.SetInteger(TypeParameter, (int)type);
            SetHealth(1f);
        }

        public void SetFlip(bool flip) {
            this.sprite.flipX = flip;
        }

        public void SetHealth(float percent) {
            this.healthBar.size = this.healthBar.size.Set(x: HealthBarSize * percent);
        }
    }
}
