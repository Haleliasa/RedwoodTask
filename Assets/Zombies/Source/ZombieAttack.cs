using UnityEngine;
using UnityEngine.Events;

namespace Zombies {
    public class ZombieAttack : MonoBehaviour {
        [SerializeField]
        private UnityEvent attacked;

        private const float InstantKillDamage = 9999f;

        private void OnCollisionEnter2D(Collision2D collision) {
            Attack(collision.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            Attack(collision.gameObject);
        }

        private void Attack(GameObject target) {
            if (!enabled
                || !target.TryGetComponentInChildren(out IHittable hittable)) {
                return;
            }
            hittable.Hit(InstantKillDamage);
            this.attacked.Invoke();
        }
    }
}
