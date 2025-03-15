using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float rotationSpeed = 500.0f;
    [Header("Ground Check Settings")]
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private Vector3 groundCheckOffset;
    [SerializeField] private LayerMask groundLayer;

    private bool isGrounded;
    private float ySpeed;
    private Quaternion targetRotation;
    private CameraController cameraController;
    private Animator animator;
    private CharacterController characterController;

    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }
    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));

        Vector3 moveInput = new Vector3(h, 0, v).normalized;

        Vector3 moveDir = cameraController.PlanarRotation * moveInput;

        GroundCheck();
        if (isGrounded)
        {
            ySpeed = -0.5f;
        }
        else
        {
            ySpeed += Physics.gravity.y * Time.deltaTime;
        }
        Vector3 velocity = moveDir * moveSpeed + Vector3.up * ySpeed;

        characterController.Move(velocity * Time.deltaTime);

        if (moveAmount > 0)
        {
            targetRotation = Quaternion.LookRotation(moveDir);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        animator.SetFloat("Speed", moveAmount);
    }
    private void GroundCheck()
    {
        Vector3 groundCheckPosition = transform.position + groundCheckOffset;
        isGrounded = Physics.CheckSphere(groundCheckPosition, groundCheckRadius, groundLayer);
    }
}
