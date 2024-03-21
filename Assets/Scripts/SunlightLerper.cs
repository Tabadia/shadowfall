using UnityEngine;

public class SunlightLerper : MonoBehaviour
{
    public Transform sunlightTransform; // Reference to the sunlight transform
    public Vector3 startRotation; // Start rotation angles
    public Vector3 endRotation; // End rotation angles
    public float lerpDuration = 2.0f; // Duration of rotation interpolation

    private bool isLerping = false; // Flag to indicate if lerping is in progress
    private float lerpStartTime; // Time when lerping started

    void Start()
    {
        StartLerp();
    }

    void Update()
    {
        if (isLerping)
        {
            // Calculate the progress of the lerp
            float lerpProgress = (Time.time - lerpStartTime) / lerpDuration;

            // Apply easing function to lerpProgress
            float t = EaseInOutQuadratic(lerpProgress);

            // Interpolate rotation between startRotation and endRotation using lerpProgress
            Quaternion targetRotation = Quaternion.Euler(Vector3.Lerp(startRotation, endRotation, t));

            // Apply the interpolated rotation to the sunlight transform
            sunlightTransform.rotation = targetRotation;

            // If lerpProgress reaches 1, stop lerping
            if (lerpProgress >= 1.0f)
            {
                isLerping = false;
            }
        }
    }

    void StartLerp()
    {
        // Set the start time of the lerp
        lerpStartTime = Time.time;

        // Set the flag to indicate that lerping is in progress
        isLerping = true;
    }

    // Custom easing function - EaseInOutQuadratic
    float EaseInOutQuadratic(float t)
    {
        return t < 0.5f ? 2f * t * t : -1f + (4f - 2f * t) * t;
    }
}
