using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 5f;
    public float sprintSpeed = 10f;
    public float sprintDuration = 3f;
    public float sprintRechargeRate = 1f;
    public float sensitivity = 2f;
    public float smoothSpeed = 5f;
    public float stabilityForce = 10f;
    public float tiltThreshold = 0.7f;
    public Transform cameraTransform;
    public GameObject flashlight;
    public float bobbingSpeed = 0.18f;
    public float bobbingAmount = 0.05f;
    public Animator animator;

    private Rigidbody rb;
    private Vector2 currentRotation;
    private Vector2 targetRotation;
    private float defaultCameraHeight;
    private float walkCycleTimer;
    private float sprintTimeLeft;
    private bool isSprinting;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = false;
        Cursor.lockState = CursorLockMode.Locked;
        defaultCameraHeight = cameraTransform.localPosition.y;
        sprintTimeLeft = sprintDuration;
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleCameraBobbing();
        FollowCameraWithFlashlight();
        RechargeSprint();
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        bool isMoving = move.magnitude > 0.1f;
        bool wantsToSprint = Input.GetKey(KeyCode.LeftShift) && sprintTimeLeft > 0;

        if (isMoving)
        {
            if (wantsToSprint)
            {
                isSprinting = true;
                sprintTimeLeft -= Time.deltaTime;
                rb.MovePosition(rb.position + move * sprintSpeed * Time.deltaTime);
            }
            else
            {
                isSprinting = false;
                rb.MovePosition(rb.position + move * speed * Time.deltaTime);
            }
        }
        else
        {
            isSprinting = false;
            rb.linearVelocity = Vector3.zero; // Prevents sliding when not moving
        }

        // Update Animator
        animator.SetBool("isWalking", isMoving && !isSprinting);
        animator.SetBool("isSprinting", isSprinting);
        animator.SetBool("isIdle", !isMoving);
    }

    void RechargeSprint()
    {
        if (!isSprinting && sprintTimeLeft < sprintDuration)
        {
            sprintTimeLeft += Time.deltaTime * sprintRechargeRate;
            sprintTimeLeft = Mathf.Clamp(sprintTimeLeft, 0, sprintDuration);
        }
    }

    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        targetRotation.x += mouseX;
        targetRotation.y -= mouseY;
        targetRotation.y = Mathf.Clamp(targetRotation.y, -90f, 90f);

        currentRotation.x = Mathf.LerpAngle(currentRotation.x, targetRotation.x, Time.deltaTime * smoothSpeed);
        currentRotation.y = Mathf.LerpAngle(currentRotation.y, targetRotation.y, Time.deltaTime * smoothSpeed);

        transform.rotation = Quaternion.Euler(0f, currentRotation.x, 0f);
        cameraTransform.localRotation = Quaternion.Euler(currentRotation.y, 0f, 0f);
    }

    void HandleCameraBobbing()
    {
        if (rb.linearVelocity.magnitude > 0.1f)
        {
            walkCycleTimer += Time.deltaTime * (isSprinting ? sprintSpeed : speed);

            float bobbingOffset = Mathf.Sin(walkCycleTimer * bobbingSpeed) * bobbingAmount;
            cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, defaultCameraHeight + bobbingOffset, cameraTransform.localPosition.z);
        }
        else
        {
            cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, defaultCameraHeight, cameraTransform.localPosition.z);
        }
    }

    void PreventSidewaysFall()
    {
        float tiltAngle = Vector3.Dot(transform.up, Vector3.up);

        if (tiltAngle < tiltThreshold)
        {
            Vector3 correctionTorque = Vector3.Cross(transform.up, Vector3.up) * stabilityForce;
            rb.AddTorque(correctionTorque);
        }
    }

    void FollowCameraWithFlashlight()
    {
        if (flashlight != null)
        {
            flashlight.transform.position = cameraTransform.position;
            flashlight.transform.rotation = cameraTransform.rotation;
        }
    }
}