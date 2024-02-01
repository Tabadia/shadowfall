using UnityEngine;

public class RotateDirectionalLight : MonoBehaviour
{
    public float rotationSpeed = 10f; // Adjust the speed of rotation as needed

    void Update()
    {
        RotateLight();
    }

    void RotateLight()
    {
        // Get the current rotation of the light
        Vector3 currentRotation = transform.rotation.eulerAngles;

        // Increment the Y coordinate to rotate around the Y-axis
        currentRotation.y += rotationSpeed * Time.deltaTime;

        // Apply the new rotation
        transform.rotation = Quaternion.Euler(currentRotation);
    }
}
