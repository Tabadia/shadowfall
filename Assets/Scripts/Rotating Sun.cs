using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using System.Collections;


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

    public bool spedUp = false;

    public GameObject flashlight;
    public GameObject dayCustomPass;
    public GameObject nightCustomPass;

    public AudioSource logAudio;
    public AudioClip[] logClips;
    public AudioClip radioClip;
    public int logIndex = 0;

    public int timesRotated = 0;
    void Start()
    {
        sunLight.SetActive(true);
        sunVol.SetActive(true);
        moonLight.SetActive(false);
        moonVol.SetActive(false);
        sunFog.SetActive(true);
        GameObject[] lightObjects = GameObject.FindGameObjectsWithTag("Light Obj");
        foreach (GameObject obj in lightObjects)
        {
            HDAdditionalLightData lightComponent = obj.GetComponent<HDAdditionalLightData>();
            if (lightComponent != null)
            {
                lightComponent.intensity *= 75;
            }
            if(obj.name == "Flashlight"){
                obj.SetActive(false);
            }
        }
        StartCoroutine(PlayRadio(0));
    }
    void Update()
    {
        
        currentMoonRotation = moonLight.transform.rotation.eulerAngles;
        currentSunRotation = sunLight.transform.rotation.eulerAngles;

        if (timesRotated == 0){
            RotateUp(sunLight, currentSunRotation);
            if(currentSunRotation.x >= 45f & !spedUp){
                rotationSpeed *= 2;
                spedUp = true;
                print("doubled");
            }
            if(currentSunRotation.x == 90f){
                timesRotated++;
            }
        }
        else if (timesRotated == 1){
            RotateDown(sunLight, currentSunRotation);
            if(currentSunRotation.x >= 135 & spedUp){
                rotationSpeed /= 2;
                spedUp = false;
            }
            if(currentSunRotation.x >= 190f){
                print("night!");
                timesRotated++;
                moonLight.SetActive(true);
                moonVol.SetActive(true);
                sunLight.SetActive(false);
                sunVol.SetActive(false);
                sunFog.SetActive(false);
                dayCustomPass.SetActive(false);
                nightCustomPass.SetActive(true);
                GameObject[] lightObjects = GameObject.FindGameObjectsWithTag("Light Obj");
                foreach (GameObject obj in lightObjects)
                {
                    Light lightComponent = obj.GetComponent<Light>();
                    if (lightComponent != null)
                    {
                        lightComponent.intensity /= 75;
                    }
                }
                if(!flashlight.activeInHierarchy){
                    flashlight.SetActive(true);
                    flashlight.GetComponent<Light>().intensity /= 75;
                    flashlight.SetActive(false);
                }
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
                nightCustomPass.SetActive(false);
                dayCustomPass.SetActive(true);
                GameObject[] lightObjects = GameObject.FindGameObjectsWithTag("Light Obj");
                foreach (GameObject obj in lightObjects)
                {
                    Light lightComponent = obj.GetComponent<Light>();
                    if (lightComponent != null)
                    {
                        lightComponent.intensity *= 75;
                    }
                    if(obj.name == "Flashlight"){
                        obj.SetActive(false);
                    }
                }
                if(!flashlight.activeInHierarchy){
                    flashlight.GetComponent<Light>().intensity *= 75;
                }
                logIndex++;
                StartCoroutine(PlayRadio(logIndex));
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

    IEnumerator PlayRadio(int index){
        yield return new WaitForSeconds(10);
        logAudio.clip = radioClip;
        logAudio.Play();
        yield return new WaitUntil(() => !logAudio.isPlaying);
        logAudio.clip = logClips[index];
        logAudio.Play();
    }
}