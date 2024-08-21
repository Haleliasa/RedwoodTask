using System.Linq;
using UnityEngine;

namespace UI {
    public class DeathMenu : MonoBehaviour, ISerializedEventListener {
        [Header(EditorHeaders.Events)]
        [SerializeField]
        private SerializedEvent[] restartEvents;

        [SerializeField]
        private SerializedEvent[] exitEvents;

        [Header(EditorHeaders.EventSubscriptions)]
        [SerializeField]
        private SerializedEvent[] showOnEvents;

        void ISerializedEventListener.OnSerializedEvent(SerializedEvent ev, Object source) {
            if (this.showOnEvents.Contains(ev)) {
                Show();
            }
        }

        public void Restart() {
            Hide();
            this.restartEvents.ForEach(e => e.Invoke(this));
        }

        public void Exit() {
            Hide();
            this.exitEvents.ForEach(e => e.Invoke(this));
        }

        private void OnEnable() {
            this.showOnEvents.ForEach(e => e.Subscribe(this));
        }

        private void Start() {
            Hide();
        }

        private void OnDestroy() {
            this.showOnEvents.ForEach(e => e.Unsubscribe(this));
        }

        private void Show() {
            gameObject.SetActive(true);
        }

        private void Hide() {
            gameObject.SetActive(false);
        }
    }
}
