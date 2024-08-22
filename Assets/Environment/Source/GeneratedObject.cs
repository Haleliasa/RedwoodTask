using System.Collections.Generic;
using UnityEngine;

namespace Environment {
    public class GeneratedObject : MonoBehaviour {
        [SerializeField]
        protected List<SpriteRenderer> flipRenderers;

        public virtual void Generate() {
            this.flipRenderers.ForEach(r => r.flipX = Random.value > 0.5f);
        }
    }
}
