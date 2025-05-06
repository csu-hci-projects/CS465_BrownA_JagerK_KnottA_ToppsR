using UnityEngine;
using UnityEngine.InputSystem;

public class JoystickDebug : MonoBehaviour
{
    public InputActionReference rightStick;

    void Update()
    {
        if (rightStick != null)
        {
            Vector2 value = rightStick.action.ReadValue<Vector2>();
            Debug.Log("Right Joystick: " + value);
        }
    }
}
