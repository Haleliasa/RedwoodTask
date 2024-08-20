
using System;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class InjectKeyAttribute : Attribute {
    public InjectKeyAttribute(string key) {
        Key = key;
    }

    public string Key { get; }
}
