using UnityEngine;

public class TouchRotateObject : MonoBehaviour
{
    public float constantRotationSpeed = 50f; // Speed for constant rotation
    public float touchRotationSpeed = 10f;   // Speed multiplier for touch rotation

    private bool isTouching = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isTouching = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isTouching = false;
        }

        if (isTouching)
        {
            // Rotate based on user input (e.g., mouse drag or touch movement)
            float horizontalInput = Input.GetAxis("Mouse X");
            float verticalInput = Input.GetAxis("Mouse Y");

            Vector3 rotationDelta = new Vector3(verticalInput, -horizontalInput, 0) * touchRotationSpeed;
            transform.Rotate(rotationDelta, Space.World);
        }
        else
        {
            // Constant rotation
            transform.Rotate(Vector3.up, constantRotationSpeed * Time.deltaTime, Space.World);
        }
    }
}
