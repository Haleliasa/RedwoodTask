
using System;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
public class InjectKeyAttribute : Attribute {
    public InjectKeyAttribute(string key) {
        Key = key;
    }

    public string Key { get; }
}
