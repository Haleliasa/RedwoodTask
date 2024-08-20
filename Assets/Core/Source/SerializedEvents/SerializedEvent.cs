using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class SerializedEvent<T> : SerializedEvent {
    [SerializeField]
    private bool useDefaultPayload = false;

    [SerializeField]
    private T defaultPayload;

    public void SubscribeTyped<TListener>(TListener listener)
        where TListener : Object, ISerializedEventListener<T> {
        SubscribeInternal(listener);
    }

    public void InvokeTyped(Object source, T payload) {
        // needed in case of subs/unsubs during event, should be optimized
        List<ISerializedEventListener<T>> listeners =
            this.listeners.OfType<ISerializedEventListener<T>>().ToList();
        foreach (ISerializedEventListener<T> listener in listeners) {
            listener.OnSerializedEvent(this, source, payload);
        }
        base.Invoke(source);
    }

    public override void Invoke(Object source) {
        if (!this.useDefaultPayload) {
            base.Invoke(source);
            return;
        }
        InvokeTyped(source, this.defaultPayload);
    }
}

[CreateAssetMenu(
    fileName = nameof(SerializedEvent),
    menuName = MenuName + nameof(SerializedEvent))]
public class SerializedEvent : ScriptableObject {
    public const string MenuName = "SerializedEvents/";

    [SerializeField]
    protected List<Object> listeners;

    public void Subscribe<TListener>(TListener listener)
        where TListener : Object, ISerializedEventListener {
        SubscribeInternal(listener);
    }

    public void Unsubscribe(Object listener) {
        this.listeners.Remove(listener);
    }

    public virtual void Invoke(Object source) {
        // needed in case of subs/unsubs during event, should be optimized
        List<ISerializedEventListener> listeners =
            this.listeners.OfType<ISerializedEventListener>().ToList();
        foreach (ISerializedEventListener listener in listeners) {
            listener.OnSerializedEvent(this, source);
        }
    }

    protected void SubscribeInternal(Object listener) {
        if (!this.listeners.Contains(listener)) {
            this.listeners.Add(listener);
        }
    }
}
