using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset[] actionAssets;

    private void Start() {
        foreach (InputActionAsset asset in this.actionAssets) {
            asset.Enable();
        }
    }
}
