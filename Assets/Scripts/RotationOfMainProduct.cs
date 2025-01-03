using UnityEngine;

public class TouchRotateObject : MonoBehaviour
{
    public float constantRotationSpeed = 50f; // Speed for constant rotation
    public float touchRotationSpeed = 10f;   // Speed multiplier for touch rotation

    private bool isTouching = false;
    private Transform targetObject;          // The object currently being touched

    void Update()
    {
        // Detect touch or mouse input
        if (Input.GetMouseButtonDown(0))
        {
            // RaycastHit hit;
            // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Check if the touched object has the "Product" tag
            // if (Physics.Raycast(ray, out hit) && hit.transform.CompareTag("Product"))
            // {
            //     isTouching = true;
            //     targetObject = hit.transform; // Assign the touched object
            // }
        }

        // if (Input.GetMouseButtonUp(0))
        // {
        //     isTouching = false;
        //     targetObject = null; // Reset the target object
        // }

        if (isTouching && targetObject != null)
        {
            // Rotate based on touch movement
            float horizontalInput = Input.GetAxis("Mouse X");
            float verticalInput = Input.GetAxis("Mouse Y");

            // Adjust rotation direction to be more intuitive
            Vector3 rotationDelta = new Vector3(-verticalInput, horizontalInput, 0) * touchRotationSpeed;
            targetObject.Rotate(rotationDelta, Space.World);
        }
        else if (targetObject == null)
        {
            // Constant rotation only for this object when not being touched
            transform.Rotate(Vector3.up, constantRotationSpeed * Time.deltaTime, Space.World);
        }
    }
}
