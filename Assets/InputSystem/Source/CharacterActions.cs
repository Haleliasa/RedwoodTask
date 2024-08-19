using UnityEngine.InputSystem;

public readonly struct CharacterActions {
    public CharacterActions(InputActionAsset asset) {
        Asset = asset;
        InputActionMap map = asset.FindActionMap("Character");
        Move = map.FindAction("Move");
        Shoot = map.FindAction("Shoot");
    }

    public InputActionAsset Asset { get; }

    public InputAction Move { get; }

    public InputAction Shoot { get; }
}
