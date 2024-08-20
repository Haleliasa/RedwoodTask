using UnityEngine;

namespace Player {
    public class PlayerHittable : MonoBehaviour, IHittable {
        public void Hit(float damage) {
            print("player died");
        }
    }
}
