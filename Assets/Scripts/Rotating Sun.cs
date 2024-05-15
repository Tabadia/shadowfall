using UnityEngine;

public class RotateDirectionalLight : MonoBehaviour
{
    public float rotationSpeed = 10f; // Adjust the speed of rotation as needed
    private Vector3 currentSunRotation;
    private Vector3 currentMoonRotation;

    public GameObject sunLight;
    public GameObject moonLight;
    public GameObject sunVol;
    public GameObject moonVol;
    public GameObject sunFog;

    public int timesRotated = 0;

    void Update()
    {
        currentMoonRotation = moonLight.transform.rotation.eulerAngles;
        currentSunRotation = sunLight.transform.rotation.eulerAngles;

        if (timesRotated == 0){
            RotateUp(sunLight, currentSunRotation);
            if(currentSunRotation.x == 90f){
                timesRotated++;
            }
        }
        else if (timesRotated == 1){
            RotateDown(sunLight, currentSunRotation);
            if(currentSunRotation.x >= 190f){
                timesRotated++;
                moonLight.SetActive(true);
                moonVol.SetActive(true);
                sunLight.SetActive(false);
                sunVol.SetActive(false);
                sunFog.SetActive(false);
            }
        }
        else if (timesRotated == 2){
            RotateUp(moonLight, currentMoonRotation);
            if(currentMoonRotation.x == 90f){
                timesRotated++;
            }
        }
        else if (timesRotated == 3){
            RotateDown(moonLight, currentMoonRotation);
            if(currentMoonRotation.x >= 190f){
                timesRotated++;
                moonLight.SetActive(false);
                moonVol.SetActive(false);
                sunLight.SetActive(true);
                sunVol.SetActive(true);
                sunFog.SetActive(true);
            }
        }
        else {
            timesRotated = 0;
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