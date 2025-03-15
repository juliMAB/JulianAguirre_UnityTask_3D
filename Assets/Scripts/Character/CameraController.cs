using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform followTarget;

    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private float distance = 5.0f;

    [SerializeField] private float minVerticalAngle = -45f;
    [SerializeField] private float maxVerticalAngle = 45;

    [SerializeField] private Vector2 framingOffset;

    float rotationX;
    float rotationY;

    

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        rotationX += Input.GetAxis("Mouse Y") * rotationSpeed;
        rotationY += Input.GetAxis("Mouse X") * rotationSpeed;
        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);

        Quaternion targetRotation = Quaternion.Euler(rotationX, rotationY, 0);

        Vector3 focusPosition = followTarget.position + Vector3.up * framingOffset.y + Vector3.right * framingOffset.x;

        transform.position = focusPosition - (targetRotation * Vector3.forward * distance);
        transform.rotation = targetRotation;
    }
    public Quaternion PlanarRotation=> Quaternion.Euler(0, rotationY, 0);
}
