using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 20.0f;

    void Update()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        var space = Input.GetKey(KeyCode.Space);

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, moveVertical) * moveSpeed * Time.deltaTime;
        if (space)
        {
            movement.z = 0;
        }
        else { movement.y = 0; }

        transform.Translate(movement, Space.World);
    }
}
