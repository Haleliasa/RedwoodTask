#nullable enable

using UnityEngine;

public class Follower : MovingObject {
    [Header(nameof(Follower))]
    [Header(EditorHeaders.References)]
    [SerializeField]
    private Transform? target;

    [Header(EditorHeaders.Properties)]
    [Tooltip("units/sec")]
    [Min(0f)]
    [SerializeField]
    private float minSpeed = 5f;

    [Tooltip("units/sec")]
    [Min(0f)]
    [SerializeField]
    private float maxSpeed = 10f;

    [Tooltip("units")]
    [Min(0f)]
    [SerializeField]
    private float minSpeedDistance = 1f;

    [Tooltip("units")]
    [Min(0f)]
    [SerializeField]
    private float maxSpeedDistance = 10f;

    protected override Vector2 GetMovement(float deltaTime) {
        if (this.target == null) {
            return Vector2.zero;
        }

        Vector2 target = this.target.position;
        Vector2 pos = transform.position;
        Vector2 dir = target - pos;
        float dist = dir.magnitude;

        if (Mathf.Approximately(dist, 0f)) {
            return Vector2.zero;
        }

        float speed = Mathf.Lerp(
            this.minSpeed,
            this.maxSpeed,
            Mathf.InverseLerp(
                this.minSpeedDistance,
                this.maxSpeedDistance,
                dist));
        float movement = speed * deltaTime;

        if (movement >= dist) {
            return dir;
        }

        return dir / dist * (speed * deltaTime);
    }
}
