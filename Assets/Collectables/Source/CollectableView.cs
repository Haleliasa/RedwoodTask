#nullable enable

using TMPro;
using UnityEngine;

namespace Collectables {
    public class CollectableView : MonoBehaviour {
        [SerializeField]
        private TMP_Text? valueText;

        public virtual void SetValue(int value) {
            if (this.valueText != null) {
                this.valueText.text = value.ToString();
            }
        }
    }
}
