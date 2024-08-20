using System;

public interface IDisposableObject<T> : IDisposable where T : UnityEngine.Object {
    T Object { get; }
}
