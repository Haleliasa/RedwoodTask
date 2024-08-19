#nullable enable

using UnityEngine;

public class MovingObject : MonoBehaviour {
    [Header(nameof(MovingObject))]
    [SerializeField]
    private new Rigidbody2D? rigidbody;

    public SimulationMode2D SimulationMode => Physics2D.simulationMode;

    public void Teleport(Vector2 position) {
        // TODO: add rigidbody teleportation
        transform.position = position;
    }
    
    protected virtual void Update() {
        if (SimulationMode == SimulationMode2D.Update) {
            Move(Time.deltaTime);
        }
    }

    protected virtual void FixedUpdate() {
        if (SimulationMode == SimulationMode2D.FixedUpdate) {
            Move(Time.fixedDeltaTime);
        }
    }

    protected virtual Vector2 Move(float deltaTime) {
        Vector2 movement = GetMovement(deltaTime);
        if (this.rigidbody != null) {
            this.rigidbody.MovePosition(this.rigidbody.position + movement);
        } else {
            transform.position += (Vector3)movement;
        }
        return movement;
    }

    protected virtual Vector2 GetMovement(float deltaTime) {
        return Vector2.zero;
    }
}
