using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class FreeCamera : MonoBehaviour
{
    public float moveSpeed = 4f;        // jednostki na sekundę
    public float fastMultiplier = 3f;   // mnożnik przy trzymanym Shift
    public bool requireRightMouse = true; // rozglądanie tylko z PPM

    float yaw, pitch;

    void Start()
    {
        Vector3 e = transform.eulerAngles;
        yaw = e.y;
        pitch = e.x > 180f ? e.x - 360f : e.x;
    }

    void Update()
    {
        Vector3 input = Vector3.zero;   // x = prawo/lewo, y = góra/dół, z = przód/tył
        Vector2 look = Vector2.zero;
        bool fast = false;
        bool looking = false;

#if ENABLE_INPUT_SYSTEM
        var kb = Keyboard.current;
        var mouse = Mouse.current;
        if (kb == null) return;

        if (kb.wKey.isPressed) input.z += 1f;
        if (kb.sKey.isPressed) input.z -= 1f;
        if (kb.dKey.isPressed) input.x += 1f;
        if (kb.aKey.isPressed) input.x -= 1f;
        if (kb.eKey.isPressed) input.y += 1f;
        if (kb.qKey.isPressed) input.y -= 1f;
        fast = kb.leftShiftKey.isPressed;

        if (mouse != null && (!requireRightMouse || mouse.rightButton.isPressed))
        {
            look = mouse.delta.ReadValue() * 0.1f;
            looking = true;
        }
#else
        if (Input.GetKey(KeyCode.W)) input.z += 1f;
        if (Input.GetKey(KeyCode.S)) input.z -= 1f;
        if (Input.GetKey(KeyCode.D)) input.x += 1f;
        if (Input.GetKey(KeyCode.A)) input.x -= 1f;
        if (Input.GetKey(KeyCode.E)) input.y += 1f;
        if (Input.GetKey(KeyCode.Q)) input.y -= 1f;
        fast = Input.GetKey(KeyCode.LeftShift);

        if (!requireRightMouse || Input.GetMouseButton(1))
        {
            look = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * 2f;
            looking = true;
        }
#endif

        // Rozglądanie się
        if (looking)
        {
            yaw += look.x;
            pitch -= look.y;
            pitch = Mathf.Clamp(pitch, -89f, 89f);
            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }

        // Ruch
        Vector3 dir = transform.right * input.x
                    + transform.forward * input.z
                    + Vector3.up * input.y;
        if (dir.sqrMagnitude > 1f) dir.Normalize();

        float speed = moveSpeed * (fast ? fastMultiplier : 1f);
        transform.position += dir * speed * Time.deltaTime;
    }
}