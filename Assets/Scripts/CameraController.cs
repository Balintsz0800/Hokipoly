using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float dragSpeed = 0.02f;
    [SerializeField] private float smoothTime = 0.1f;
    
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 25f;

    [SerializeField] private Vector2 xBounds = new Vector2(-10f, 10f);
    [SerializeField] private Vector2 zBounds = new Vector2(-10f, 10f);

    private Camera cam;
    private Vector3 targetPosition;
    private Vector3 velocity;

    private Vector3 lastMousePosition;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        targetPosition = transform.position;
        targetPosition = ClampPosition(targetPosition);
    }

    private void Update()
    {
        HandleDrag();
        HandleZoom();
    }

    private void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            smoothTime
        );
    }

    private void HandleDrag()
    {
        if (Input.GetMouseButtonDown(1))
            lastMousePosition = Input.mousePosition;

        if (!Input.GetMouseButton(1))
            return;

        Vector3 delta = Input.mousePosition - lastMousePosition;
        lastMousePosition = Input.mousePosition;

        Vector3 right = transform.right;
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;

        Vector3 movement =
            (-right * delta.x - forward * delta.y) * dragSpeed;

        targetPosition += movement;
        targetPosition = ClampPosition(targetPosition);
    }

    private void HandleZoom()
    {
        float scroll = Input.mouseScrollDelta.y;

        if (Mathf.Approximately(scroll, 0f))
            return;

        targetPosition += Vector3.up * (-scroll * zoomSpeed);
        targetPosition = ClampPosition(targetPosition);
    }

    private Vector3 ClampPosition(Vector3 position)
    {
        position.x = Mathf.Clamp(position.x, xBounds.x, xBounds.y);
        position.y = Mathf.Clamp(position.y, minZoom, maxZoom);
        position.z = Mathf.Clamp(position.z, zBounds.x, zBounds.y);

        return position;
    }
}