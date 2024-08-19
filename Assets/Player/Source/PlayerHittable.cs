using UnityEngine;

namespace Player {
    public class PlayerHittable : MonoBehaviour, IHittable {
        void IHittable.Hit(float damage) {
            print("player died");
        }
    }
}
