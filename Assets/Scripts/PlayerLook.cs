using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [Header("Look Settings")]
    public float sensitivityX = 2f;
    public float sensitivityY = 2f;

    [Header("References")]
    public Transform playerBody; // the capsule (moves left/right)
    public Transform cameraTransform; // the camera (looks up/down)

    private Vector2 lookInput;
    private float xRotation = 0f;

    // Called by Player Input when Look action is triggered
    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = lookInput.x * sensitivityX * Time.deltaTime;
        float mouseY = lookInput.y * sensitivityY * Time.deltaTime;

        // Rotate camera up/down (pitch)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f); // stops full backflip
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotate player body left/right (yaw)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
