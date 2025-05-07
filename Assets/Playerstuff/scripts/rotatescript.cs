using UnityEngine;

public class rotatescript : MonoBehaviour
{
    public float sensitivity = 2f;
    public float smoothSpeed = 5f; // Lower value = more laggy effect
    public Transform playerBody;

    private float xRotation = 0f;
    private Vector2 currentRotation;
    private Vector2 rotationVelocity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetMouseButton(1)) // Right mouse button
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

            // Target rotation values
            currentRotation.x += mouseX;
            currentRotation.y -= mouseY;
            currentRotation.y = Mathf.Clamp(currentRotation.y, -90f, 90f);

            // Smoothly interpolate rotation
            float smoothX = Mathf.LerpAngle(transform.localEulerAngles.x, currentRotation.y, Time.deltaTime * smoothSpeed);
            float smoothY = Mathf.LerpAngle(playerBody.localEulerAngles.y, currentRotation.x, Time.deltaTime * smoothSpeed);

            // Apply smoothed rotations
            transform.localRotation = Quaternion.Euler(smoothX, 0f, 0f);
            playerBody.localRotation = Quaternion.Euler(0f, smoothY, 0f);
        }
    }
}