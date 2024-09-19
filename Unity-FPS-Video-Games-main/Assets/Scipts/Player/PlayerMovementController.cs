using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float runMultiplier = 2f; // Multiplicador para correr
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float jumpRaycastDistance = 1.1f;
    [SerializeField] private Transform spawnPoint;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.position = spawnPoint.position;
    }

    private void Update()
    {
        Jump();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float hAxis = Input.GetAxisRaw("Horizontal");
        float vAxis = Input.GetAxisRaw("Vertical");

        // Determinar si se está presionando el shift para correr
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? speed * runMultiplier : speed;

        Vector3 movement = new Vector3(hAxis, 0, vAxis) * currentSpeed * Time.fixedDeltaTime;

        Vector3 newPosition = rb.position + rb.transform.TransformDirection(movement);

        rb.MovePosition(newPosition);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsGrounded())
            {
                rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
            }
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, jumpRaycastDistance);
    }
}
