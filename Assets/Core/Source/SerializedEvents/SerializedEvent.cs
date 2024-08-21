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
        IEnumerable<Object> typedListenerObjects = AllListeners.Where(listener =>
            listener is ISerializedEventListener<T>);
        // list copy needed in case of subs/unsubs during event, should be optimized
        List<ISerializedEventListener<T>> listeners =
            typedListenerObjects.OfType<ISerializedEventListener<T>>().ToList();
        foreach (ISerializedEventListener<T> listener in listeners) {
            listener.OnSerializedEvent(this, source, payload);
        }
        InvokeInternal(
            source,
            AllListeners.Except(typedListenerObjects),
            log: false,
            out int untypedListenerCount);
        Debug.Log($"Serialized event {name} invoked by {source.name}" +
            $"\n\twith payload {payload} ({payload.GetType()})" +
            $"\n\twith {listeners.Count} typed listeners" +
            $"\n\tand {untypedListenerCount} untyped listeners");
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
public class SerializedEvent : ScriptableObject, IRuntimeAsset {
    public const string MenuName = "SerializedEvents/";

    [Tooltip("only assets can be predefined listeners")]
    [SerializeField]
    private Object[] predefinedListeners;

    private List<Object> runtimeListeners = null!;

#if UNITY_EDITOR
    public IReadOnlyList<Object> RuntimeListeners => this.runtimeListeners;
#endif

    protected IEnumerable<Object> AllListeners =>
        this.predefinedListeners.Concat(this.runtimeListeners);

    public void Subscribe<TListener>(TListener listener)
        where TListener : Object, ISerializedEventListener {
        SubscribeInternal(listener);
    }

    public void Unsubscribe(Object listener) {
        this.runtimeListeners.Remove(listener);
    }

    public virtual void Invoke(Object source) {
        InvokeInternal(source, AllListeners, log: true, out _);
    }

    protected void SubscribeInternal(Object listener) {
        if (!this.runtimeListeners.Contains(listener)) {
            this.runtimeListeners.Add(listener);
        }
    }

    protected void InvokeInternal(
        Object source,
        IEnumerable<Object> listenerObjects,
        bool log,
        out int listenerCount) {
        // list copy needed in case of subs/unsubs during event, should be optimized
        List<ISerializedEventListener> listeners =
            listenerObjects.OfType<ISerializedEventListener>().ToList();
        listenerCount = listeners.Count;
        foreach (ISerializedEventListener listener in listeners) {
            listener.OnSerializedEvent(this, source);
        }
        if (log) {
            Debug.Log($"Serialized event {name} invoked by {source.name}" +
                $"\n\twith {listenerCount} listeners");
        }
    }

    protected void Awake() {
        this.runtimeListeners ??= new List<Object>();
    }

    protected void OnDestroy() {
        this.runtimeListeners.Clear();
    }
}
