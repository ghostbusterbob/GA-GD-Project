using UnityEngine;

public class JumpForce : MonoBehaviour
{
    public float jumpForce = 10f;
    public float jumpCooldown = 1f;
    private Rigidbody rb;
    private float lastJumpTime;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastJumpTime + jumpCooldown)
        {
            CorrectUprightAndJump();
        }
    }
    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        lastJumpTime = Time.time;
    }
    void CorrectUprightAndJump()
    {
        transform.position += Vector3.up;
        Jump();
    }
}
