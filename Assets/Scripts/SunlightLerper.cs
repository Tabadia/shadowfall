using UnityEngine;

public class SunlightLerper : MonoBehaviour
{
    public Transform sunlightTransform; // Reference to the sunlight transform

    // Rotation variables
    public Vector3 startRotation; // Start rotation angles
    public Vector3 endRotation1; // End rotation angles for the first transition
    public Vector3 endRotation2; // End rotation angles for the second transition
    public Vector3 endRotation3; // End rotation angles for the third transition

    // Duration variables
    public float lerpDuration1 = 2.0f; // Duration of rotation interpolation for the first transition
    public float lerpDuration2 = 2.0f; // Duration of rotation interpolation for the second transition
    public float lerpDuration3 = 2.0f; // Duration of rotation interpolation for the third transition

    private bool isLerping = false; // Flag to indicate if lerping is in progress
    private float lerpStartTime; // Time when lerping started
    private int currentTransition = 1; // Track the current transition

    void Start()
    {
        StartLerp();
    }

    void Update()
    {
        if (isLerping)
        {
            // Calculate the progress of the lerp based on the current transition
            float lerpProgress;
            switch (currentTransition)
            {
                case 1:
                    lerpProgress = (Time.time - lerpStartTime) / lerpDuration1;
                    break;
                case 2:
                    lerpProgress = (Time.time - lerpStartTime) / lerpDuration2;
                    break;
                case 3:
                    lerpProgress = (Time.time - lerpStartTime) / lerpDuration3;
                    break;
                default:
                    lerpProgress = 0f;
                    break;
            }

            // Apply easing function to lerpProgress
            float t = EaseInOutQuadratic(lerpProgress);

            // Interpolate rotation based on the current transition
            Vector3 targetRotation;
            switch (currentTransition)
            {
                case 1:
                    targetRotation = Vector3.Lerp(startRotation, endRotation1, t);
                    break;
                case 2:
                    targetRotation = Vector3.Lerp(endRotation1, endRotation2, t);
                    break;
                case 3:
                    targetRotation = Vector3.Lerp(endRotation2, endRotation3, t);
                    break;
                default:
                    targetRotation = Vector3.zero;
                    break;
            }

            // Apply the interpolated rotation to the sunlight transform
            sunlightTransform.rotation = Quaternion.Euler(targetRotation);

            // If lerpProgress reaches 1, switch to the next transition
            if (lerpProgress >= 1.0f)
            {
                currentTransition++;
                if (currentTransition > 3)
                {
                    currentTransition = 1;
                }
                StartLerp();
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
