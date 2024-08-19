#nullable enable

using UnityEngine;

namespace Player {
    public class PlayerView : MonoBehaviour {
        [SerializeField]
        private Animator animator = null!;

        [SerializeField]
        private SpriteRenderer[] sprites = null!;

        [SerializeField]
        private Transform[] flipPositions = null!;

        private bool flip = false;

        public void SetMoveAxis(float axis) {
            this.animator.SetBool(
                PlayerAnimatorParameters.Moving,
                !Mathf.Approximately(axis, 0f));
            bool flip = axis < 0f;
            if (flip != this.flip) {
                foreach (SpriteRenderer sprite in this.sprites) {
                    sprite.flipX = flip;
                }
                foreach (Transform obj in this.flipPositions) {
                    Vector3 pos = obj.localPosition;
                    obj.localPosition = pos.Set(x: -pos.x);
                }
                this.flip = flip;
            }
        }

        public void TriggerShoot() {
            this.animator.SetTrigger(PlayerAnimatorParameters.Shoot);
        }
    }
}
