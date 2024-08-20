#nullable enable

using System.Collections;
using UnityEngine;

namespace Player {
    public class PlayerView : MonoBehaviour {
        [Header(EditorHeaders.References)]
        [SerializeField]
        private Animator animator = null!;

        [SerializeField]
        private SpriteRenderer[] sprites = null!;

        [SerializeField]
        private Transform[] flipPositions = null!;

        [Header(EditorHeaders.Properties)]
        [Tooltip("sec")]
        [Min(0f)]
        [SerializeField]
        private float defaultShootDuration = 0.5f;

        [SerializeField]
        private bool shootOnEvent = false;

        private bool flip = false;
        private bool waitingShoot = false;

        public void SetMoveAxis(float axis) {
            bool moving = !Mathf.Approximately(axis, 0f);
            this.animator.SetBool(PlayerAnimatorParameters.Moving, moving);
            
            if (!moving) {
                return;
            }

            bool flip = axis < 0f;

            if (flip == this.flip) {
                return;
            }

            foreach (SpriteRenderer sprite in this.sprites) {
                sprite.flipX ^= true;
            }
            foreach (Transform obj in this.flipPositions) {
                Vector3 pos = obj.localPosition;
                obj.localPosition = pos.Set(x: -pos.x);
            }
            this.flip = flip;
        }

        public IEnumerator Shoot(float duration) {
            this.animator.SetFloat(
                PlayerAnimatorParameters.ShootSpeed,
                duration != 0f
                    ? (this.defaultShootDuration / duration)
                    : 1f);
            this.animator.SetTrigger(PlayerAnimatorParameters.Shoot);

            if (!this.shootOnEvent) {
                yield break;
            }

            this.waitingShoot = true;
            yield return new WaitWhile(() => this.waitingShoot);
        }

        private void OnShoot() {
            this.waitingShoot = false;
        }
    }
}
