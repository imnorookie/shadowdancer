using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource walkingSound;
    
    [SerializeField]
    private AudioClip defaultWalkingSound;
    
    private AudioClip currentWalkingSound;
    [SerializeField]
    public AudioClip[] walkingSounds;

    [Range(0.0f, 1.0f)]
    public float walkingSoundVolume = 1.0f;

    private float crouchingSoundPitch = 1f;

    private float walkingSoundPitch = 2f;

    private float sprintingSoundPitch = 3f;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Player Sound Manager Initialized");
        walkingSound.volume = walkingSoundVolume;
    }

    public bool isSoundPlaying()
    {
        return walkingSound.isPlaying;
    }

    public void PlayWalkingSound() {
        if (!walkingSound.isPlaying) 
            walkingSound.Play();
    }

    public void StopWalkingSound() {
        walkingSound.Stop();
    }

    public void PlayDefaultWalkingSound() {
        walkingSound.clip = walkingSounds[0];
        PlayWalkingSound();
    }

    public void switchClip(int index)
    {
        walkingSound.Pause();

        if (index < 0 || index >= walkingSounds.Length)
        {
            Debug.Log("Index out of range");
            walkingSound.clip = walkingSounds[0];
        } else
        {
            walkingSound.clip = walkingSounds[index];
        }
        walkingSound.Play();
    }

    public void SetWalkingSoundVolume(float volume) {
        walkingSoundVolume = volume;
        walkingSound.volume = walkingSoundVolume;
    }

    public void SetWalkingSoundPitch() {
        walkingSound.pitch = walkingSoundPitch;
    }

    public void SetCrouchingSoundPitch() {
        walkingSound.pitch = crouchingSoundPitch;
    }

    public void SetSprintingSoundPitch()
    {
        walkingSound.pitch = sprintingSoundPitch;
    }

}
