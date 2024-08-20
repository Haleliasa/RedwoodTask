using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour,
    ISerializedEventListener,
    ISerializedEventListener<int> {
    [SerializeField]
    private SerializedEvent[] gameOverOnEvents;

    [SerializeField]
    private SerializedEvent<int>[] gameOverAtZeroOnEvents;

    void ISerializedEventListener.OnSerializedEvent(SerializedEvent ev, Object source) {
        if (this.gameOverOnEvents.Contains(ev)) {
            Over();
        }
    }

    void ISerializedEventListener<int>.OnSerializedEvent(
        SerializedEvent<int> ev,
        Object source,
        int payload) {
        if (this.gameOverAtZeroOnEvents.Contains(ev)
            && payload == 0) {
            Over();
        }
    }

    private void OnEnable() {
        this.gameOverOnEvents.ForEach(ev => ev.Subscribe(this));
        this.gameOverAtZeroOnEvents.ForEach(ev => ev.SubscribeTyped(this));
    }

    private void OnDisable() {
        this.gameOverOnEvents.ForEach(ev => ev.Unsubscribe(this));
        this.gameOverAtZeroOnEvents.ForEach(ev => ev.Unsubscribe(this));
    }

    private void Over() {
        print("game over");
    }
}
