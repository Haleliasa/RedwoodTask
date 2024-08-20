using UnityEngine;

namespace Player {
    public class PlayerHittable : MonoBehaviour, IHittable {
        [SerializeField]
        private SerializedEvent[] diedEvents;

        public void Hit(float damage) {
            this.diedEvents.ForEach(e => e.Invoke(this));
        }
    }
}
