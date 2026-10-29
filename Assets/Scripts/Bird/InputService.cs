using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    public class InputService : MonoBehaviour
    {
        public bool JumpPressedThisFrame =>
            Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame;

        public bool ShootPressedThisFrame =>
            Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame;

        public Vector2 PointerPosition =>
            Mouse.current != null
                ? Mouse.current.position.ReadValue()
                : Vector2.zero;
    }
}
