using UnityEngine;

namespace Environment {
    public class GeneratedBuilding : GeneratedObject {
        [Header(nameof(GeneratedBuilding))]
        [Header(EditorHeaders.References)]
        [SerializeField]
        private SpriteRenderer blockPrefab;

        [Header(EditorHeaders.Properties)]
        [Tooltip("blocks")]
        [Min(1)]
        [SerializeField]
        private int minWidth = 2;

        [Tooltip("blocks")]
        [Min(1)]
        [SerializeField]
        private int maxWidth = 5;

        [Tooltip("blocks")]
        [Min(1)]
        [SerializeField]
        private int minHeight = 1;

        [Tooltip("blocks")]
        [Min(1)]
        [SerializeField]
        private int maxHeight = 3;

        public override void Generate() {
            Vector2 blockSize = this.blockPrefab.bounds.size;
            int width = Random.Range(this.minWidth, this.maxWidth + 1);
            int height = Random.Range(this.minHeight, this.maxHeight + 1);
            int x = 0;
            int y = 0;
            while (width > 0 && y < height) {
                for (int xi = x; xi < width; xi++) {
                    SpriteRenderer block = Instantiate(this.blockPrefab, transform);
                    block.transform.localPosition =
                        new Vector3(xi * blockSize.x, y * blockSize.y, 0);
                    this.flipRenderers.Add(block);
                }
                int nextWidth = Random.Range(1, width + 1);
                x += Random.Range(0, width - nextWidth + 1);
                width = nextWidth;
                y++;
            }
            base.Generate();
        }
    }
}
