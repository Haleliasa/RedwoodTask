using System;
using System.Linq;
using UnityEngine;

namespace Environment {
    public class GeneratedEnvironment : MonoBehaviour, IInjectComponent {
        [SerializeField]
        private GeneratedObjectSetting[] generatedObjects;

        [SerializeField]
        private SpriteRenderer[] tiledRenderers;

        private new Camera camera;
        private (float left, float right)[] generatedXRange;
        private float[] tiledRendererWidths;
        private float startCameraX;

        [Inject]
        public void Inject(Camera camera) {
            this.camera = camera;
        }

        private void Start() {
            this.generatedXRange = this.generatedObjects.Select(_ => (0f, 0f)).ToArray();
            this.tiledRendererWidths = this.tiledRenderers.Select(r => r.size.x).ToArray();
            this.startCameraX = transform.InverseTransformPoint(this.camera.transform.position).x;
        }

        private void Update() {
            float cameraX = transform.InverseTransformPoint(this.camera.transform.position).x;

            for (int i = 0; i < this.generatedObjects.Length; i++) {
                float interval = this.generatedObjects[i].xInterval;
                float probability = this.generatedObjects[i].probability;
                (float xLeft, float xRight) = this.generatedXRange[i];

                if (cameraX < xLeft) {
                    xLeft -= interval;
                    if (UnityEngine.Random.value <= probability) {
                        Generate(this.generatedObjects[i], xLeft);
                    }
                }

                if (cameraX > xRight) {
                    xRight += interval;
                    if (UnityEngine.Random.value <= probability) {
                        Generate(this.generatedObjects[i], xRight);
                    }
                }

                this.generatedXRange[i] = (xLeft, xRight);
            }

            float cameraDistFromStart = Mathf.Abs(cameraX - this.startCameraX);
            this.tiledRenderers.ForEach((r, i) => {
                int chunksTraveled = (int)(cameraDistFromStart / this.tiledRendererWidths[i]);
                // +3 is for the extra 1 chunk on the left and right
                r.size = r.size.Set(x: this.tiledRendererWidths[i] * ((chunksTraveled * 2) + 3));
            });
        }

        private void Generate(GeneratedObjectSetting setting, float x) {
            float y = UnityEngine.Random.Range(setting.minY, setting.maxY);
            GeneratedObject obj = Instantiate(setting.prefab, transform);
            obj.transform.localPosition = new Vector3(x, y, 0f);
            obj.Generate();
        }

        [Serializable]
        private struct GeneratedObjectSetting {
            [SerializeField]
            public GeneratedObject prefab;

            [Tooltip("units")]
            [Min(0f)]
            [SerializeField]
            public float xInterval;

            [Range(0f, 1f)]
            [SerializeField]
            public float probability;

            [Tooltip("units")]
            [SerializeField]
            public float minY;

            [Tooltip("units")]
            [SerializeField]
            public float maxY;
        }
    }
}
