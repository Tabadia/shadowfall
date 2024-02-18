using UnityEngine;

public class RotateDirectionalLight : MonoBehaviour
{
    public float rotationSpeed = 10f; // Adjust the speed of rotation as needed
    private Vector3 currentSunRotation;
    private Vector3 currentMoonRotation;
    private bool rotatingUp = true;

    public GameObject sunLight;
    public GameObject moonLight;

    void Update()
    {
        if (rotatingUp)
        {
            currentSunRotation = sunLight.transform.rotation.eulerAngles;
            if (currentSunRotation.x >= 90)
            {
                rotatingUp = false;
                sunLight.SetActive(false);
                moonLight.SetActive(true);
            }
            else
            {
                RotateUp(sunLight, currentSunRotation);
            }
        }
        else
        {
            currentMoonRotation = moonLight.transform.rotation.eulerAngles;
            if (currentMoonRotation.x <= -4)
            {
                print("mooned");
                rotatingUp = true;
                moonLight.SetActive(false);
                sunLight.SetActive(true);
            }
            else
            {
                RotateDown(moonLight, currentMoonRotation);
            }
        }
    }

    void RotateUp(GameObject light, Vector3 currentRotation) {
        // Increment the Y coordinate to rotate around the Y-axis
        currentRotation.y += rotationSpeed * Time.deltaTime;
        currentRotation.x += rotationSpeed/4 * Time.deltaTime;

        // Apply the new rotation
        light.transform.rotation = Quaternion.Euler(currentRotation);
    }

    void RotateDown(GameObject light, Vector3 currentRotation) {
        // Increment the Y coordinate to rotate around the Y-axis
        currentRotation.y += rotationSpeed * Time.deltaTime;
        currentRotation.x -= rotationSpeed/4 * Time.deltaTime;

        // Apply the new rotation
        light.transform.rotation = Quaternion.Euler(currentRotation);
    }
}