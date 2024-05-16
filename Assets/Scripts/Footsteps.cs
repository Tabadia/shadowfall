using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public PlayerController playerController;
    public CharacterController characterController;
    public AudioSource audioSource;
    public AudioClip[] footstepSounds;
    public AudioClip[] sandFootstepSounds;
    private float nextFootstepTime = 0f;

    public bool onSand = true;

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (playerController.isMoving && !playerController.crouching && characterController.isGrounded)
        {
            float footstepRate = playerController.isRunning ? 0.2f : 0.4f;
            if (Time.time >= nextFootstepTime)
            {
                RaycastHit hit;
                if (Physics.Raycast(player.transform.position, Vector3.down, out hit))
                {
                    if (hit.collider.CompareTag("Sand"))
                    {
                        onSand = true;
                        PlaySandFootstepSound();
                    }
                    else
                    {
                        onSand = false;
                        PlayFootstepSound();
                    }
                }
                
                nextFootstepTime = Time.time + footstepRate;
            }
        }
    }

    void PlayFootstepSound()
    {
        AudioClip clip = footstepSounds[Random.Range(0, footstepSounds.Length)];
        audioSource.PlayOneShot(clip);
    }

    void PlaySandFootstepSound()
    {
        AudioClip clip = sandFootstepSounds[Random.Range(0, sandFootstepSounds.Length)];
        audioSource.PlayOneShot(clip);
    }
}
