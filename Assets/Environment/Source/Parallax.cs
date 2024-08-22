using UnityEngine;

namespace Environment {
    public class Parallax : MonoBehaviour, IInjectComponent {
        [SerializeField]
        private Transform anchor;

        [Min(0f)]
        [SerializeField]
        private float speedFraction = 0.5f;

        private new Camera camera;

        [Inject]
        public void Inject(Camera camera) {
            this.camera = camera;
        }

        private void Update() {
            Vector2 toAnchor = this.anchor.position - this.camera.transform.position;
            transform.position = (Vector2)this.camera.transform.position
                + (toAnchor * this.speedFraction);
        }
    }
}
