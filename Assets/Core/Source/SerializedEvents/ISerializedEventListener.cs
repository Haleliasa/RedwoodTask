using UnityEngine;

public interface ISerializedEventListener<T> {
    void OnSerializedEvent(SerializedEvent<T> ev, Object source, T payload);
}

public interface ISerializedEventListener {
    void OnSerializedEvent(SerializedEvent ev, Object source);
}
