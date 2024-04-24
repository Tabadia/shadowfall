using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public PlayerController playerController;
    public CharacterController characterController;
    public AudioSource audioSource;
    public AudioClip[] footstepSounds;
    private float nextFootstepTime = 0f;

    void Update()
    {
        if (playerController.isMoving && !playerController.crouching && characterController.isGrounded)
        {
            float footstepRate = playerController.isRunning ? 0.2f : 0.4f;
            if (Time.time >= nextFootstepTime)
            {
                PlayFootstepSound();
                nextFootstepTime = Time.time + footstepRate;
            }
        }
    }

    void PlayFootstepSound()
    {
        AudioClip clip = footstepSounds[Random.Range(0, footstepSounds.Length)];
        audioSource.PlayOneShot(clip);
    }
}
