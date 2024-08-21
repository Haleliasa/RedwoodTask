using System.Linq;
using TMPro;
using UnityEngine;

namespace UI {
    public class ResourceView : MonoBehaviour, ISerializedEventListener<int> {
        [Header(EditorHeaders.References)]
        [SerializeField]
        private TMP_Text text;

        [Header(EditorHeaders.EventSubscriptions)]
        [SerializeField]
        private SerializedEvent<int>[] updateOnEvents;

        private void OnEnable() {
            this.updateOnEvents.ForEach(e => e.SubscribeTyped(this));
        }

        private void OnDisable() {
            this.updateOnEvents.ForEach(e => e.Unsubscribe(this));
        }

        void ISerializedEventListener<int>.OnSerializedEvent(
            SerializedEvent<int> ev,
            Object source,
            int payload) {
            if (this.updateOnEvents.Contains(ev)) {
                this.text.text = payload.ToString();
            }
        }
    }
}
