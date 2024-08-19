using UnityEngine;

namespace Zombies {
    public class ZombieAttack : MonoBehaviour {
        private const float InstantKillDamage = 9999f;

        private void OnCollisionEnter2D(Collision2D collision) {
            Attack(collision.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            Attack(collision.gameObject);
        }

        private void Attack(GameObject target) {
            if (!target.TryGetComponentInChildren(out IHittable hittable)) {
                return;
            }
            hittable.Hit(InstantKillDamage);
        }
    }
}
