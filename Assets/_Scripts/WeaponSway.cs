using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    public float bobbingSpeed = 10f;
    public float bobbingAmount = 0.05f;
    public float horizontalBobbingAmount = 0.02f;
    public Vector3 midpoint;

    private float timer = 0.0f;
    private Vector3 originalPosition;
    private float verticalRandomOffset;
    private float horizontalRandomOffset;

    [SerializeField] private float swaySmooth = 6f;
    [SerializeField] private float swayAmount = 0.02f;
    [SerializeField] private float fallSwayAmount = 0.05f;
    [SerializeField] private float maxSwayDistance = 0.1f;

    [SerializeField] private float rotationSwayAmount = 2f;
    [SerializeField] private float rotationSwaySmooth = 6f;
    [SerializeField] private float fallTiltAmount = 5f;

    private Recoi recoilScript;
    public Rigidbody playerRigidbody;

    void Start()
    {
        originalPosition = transform.localPosition;
        midpoint = originalPosition;

        verticalRandomOffset = Random.Range(0f, Mathf.PI * 2);
        horizontalRandomOffset = Random.Range(0f, Mathf.PI * 2);

        recoilScript = GetComponent<Recoi>();
    }

    void Update()
    {
        // Adjust bobbing speed for sprinting
        bobbingSpeed = Input.GetKey(KeyCode.LeftShift) ? 23f : 18f;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        // Bobbing calculation
        timer += bobbingSpeed * Time.deltaTime;
        if (timer > Mathf.PI * 2) timer -= Mathf.PI * 2;

        float waveSlice = Mathf.Sin(timer + verticalRandomOffset);
        float verticalOffset = waveSlice * bobbingAmount * Random.Range(0.9f, 1.1f);
        float horizontalOffset = Mathf.Cos(timer + horizontalRandomOffset) * horizontalBobbingAmount * Random.Range(0.9f, 1.1f);

        float totalAxes = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

        Vector3 swayPosition = new Vector3(
            midpoint.x + (horizontalOffset * totalAxes),
            midpoint.y + (verticalOffset * totalAxes),
            midpoint.z
        );

        // Mouse sway for position (inverted)
        Vector3 mouseSway = new Vector3(
            -mouseX * swayAmount,
            -mouseY * swayAmount,
            0
        );
        mouseSway = Vector3.ClampMagnitude(mouseSway, maxSwayDistance);

        // Recoil
        Vector3 recoilOffset = recoilScript != null ? recoilScript.GetRecoilOffset() : Vector3.zero;
        Quaternion recoilRot = recoilScript != null ? recoilScript.GetRecoilRotation() : Quaternion.identity;

        // Fall effect
        Vector3 fallEffect = Vector3.zero;
        Quaternion fallTilt = Quaternion.identity;
        if (playerRigidbody != null && playerRigidbody.linearVelocity.y < -1f)
        {
            float fallIntensity = Mathf.Abs(playerRigidbody.linearVelocity.y);
            fallEffect = new Vector3(0f, fallIntensity * 0.05f, 0f);

            float fallAngle = Mathf.Sin(Time.time * 5f) * fallTiltAmount * (fallIntensity * 0.02f);
            fallTilt = Quaternion.Euler(fallAngle, 0f, fallAngle * 0.5f);
        }

        // Final position
        Vector3 targetPosition = swayPosition + mouseSway + recoilOffset + fallEffect;
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * swaySmooth);

        // Mouse sway for rotation
        Quaternion swayRotX = Quaternion.AngleAxis(-mouseY * rotationSwayAmount, Vector3.right);
        Quaternion swayRotY = Quaternion.AngleAxis(mouseX * rotationSwayAmount, Vector3.up);
        Quaternion swayRotation = swayRotX * swayRotY;

        // Final rotation
        Quaternion targetRotation = swayRotation * recoilRot * fallTilt;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * rotationSwaySmooth);
    }
}
