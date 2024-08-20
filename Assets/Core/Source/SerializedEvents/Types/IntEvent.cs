using UnityEngine;

[CreateAssetMenu(
    fileName = nameof(IntEvent),
    menuName = MenuName + nameof(IntEvent))]
public class IntEvent : SerializedEvent<int> { }
