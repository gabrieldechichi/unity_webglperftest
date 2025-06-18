using UnityEngine;

[DefaultExecutionOrder(int.MaxValue)]
public class CameraController : MonoBehaviour
{
    public float maxCamSpeed = 25f;
    public float minCamSpeed = 10f;
    public float rotateInputScale = 0.1f;
    public float maxPitch = 60f;
    public float minPitch = -60f;

    public float pitch = 45f;
    public float yaw = 0f;
    private bool isMouseLocked = false;

    void Update()
    {
        HandleMouseInput();
        HandleMovement();
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isMouseLocked = true;
            // Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isMouseLocked = false;
            // Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (isMouseLocked)
        {
            float mouseDeltaX = Input.GetAxis("Mouse X");
            float mouseDeltaY = Input.GetAxis("Mouse Y");

            pitch -= mouseDeltaY * rotateInputScale;
            yaw += mouseDeltaX * rotateInputScale;

            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }

        transform.rotation = Quaternion.Euler(pitch, yaw, 0);
    }

    void HandleMovement()
    {
        float t = Mathf.InverseLerp(0, 20, Mathf.Abs(transform.position.y));
        float camSpeed = Mathf.Lerp(minCamSpeed, maxCamSpeed, t);

        Vector3 moveInput = Vector3.zero;

        if (Input.GetKey(KeyCode.A))
            moveInput.x -= camSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D))
            moveInput.x += camSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.W))
            moveInput.z += camSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S))
            moveInput.z -= camSpeed * Time.deltaTime;

        Vector3 move = transform.rotation * moveInput;
        move.Normalize();
        move *= camSpeed * Time.deltaTime;

        transform.position += move;
    }
}
