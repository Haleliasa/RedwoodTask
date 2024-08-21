using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour,
    ISerializedEventListener,
    ISerializedEventListener<int> {
    [Header(EditorHeaders.Events)]
    [SerializeField]
    private SerializedEvent[] startedEvents;

    [SerializeField]
    private SerializedEvent[] stoppedEvents;

    [Header(EditorHeaders.EventSubscriptions)]
    [SerializeField]
    private SerializedEvent[] stopOnEvents;

    [SerializeField]
    private SerializedEvent<int>[] stopAtZeroOnEvents;

    [SerializeField]
    private SerializedEvent[] restartOnEvents;

    [SerializeField]
    private SerializedEvent[] exitOnEvents;

    void ISerializedEventListener.OnSerializedEvent(SerializedEvent ev, Object source) {
        if (this.stopOnEvents.Contains(ev)) {
            StopGame();
        } else if (this.restartOnEvents.Contains(ev)) {
            StartGame();
        } else if (this.exitOnEvents.Contains(ev)) {
            Exit();
        }
    }

    void ISerializedEventListener<int>.OnSerializedEvent(
        SerializedEvent<int> ev,
        Object source,
        int payload) {
        if (this.stopAtZeroOnEvents.Contains(ev)
            && payload == 0) {
            StopGame();
        }
    }

    private void OnEnable() {
        this.stopOnEvents.ForEach(ev => ev.Subscribe(this));
        this.stopAtZeroOnEvents.ForEach(ev => ev.SubscribeTyped(this));
        this.restartOnEvents.ForEach(ev => ev.Subscribe(this));
        this.exitOnEvents.ForEach(ev => ev.Subscribe(this));
    }

    private void Start() {
        StartGame();
    }

    private void OnDisable() {
        this.stopOnEvents.ForEach(ev => ev.Unsubscribe(this));
        this.stopAtZeroOnEvents.ForEach(ev => ev.Unsubscribe(this));
        this.restartOnEvents.ForEach(ev => ev.Unsubscribe(this));
        this.exitOnEvents.ForEach(ev => ev.Unsubscribe(this));
    }

    private void StartGame() {
        this.startedEvents.ForEach(ev => ev.Invoke(this));
    }

    private void StopGame() {
        this.stoppedEvents.ForEach(ev => ev.Invoke(this));
    }

    private void Exit() {
        Application.Quit();
    }
}
