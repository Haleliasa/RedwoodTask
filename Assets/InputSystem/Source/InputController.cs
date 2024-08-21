using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset[] actionAssets;

    private void Start() {
        this.actionAssets.ForEach(a => a.Enable());
    }
}
