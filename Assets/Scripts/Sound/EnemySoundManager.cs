using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource enemySound;

    [SerializeField]
    private AudioClip idelClip;

    [SerializeField]
    private AudioClip walkingClip;

    [SerializeField]
    private AudioClip runningClip;

    [SerializeField]
    private AudioClip attackClip;

    [SerializeField]
    private AudioClip alertClip;

    void Start()
    {
        
    }

    public void PlayIdelSound()
    {
        if (enemySound.clip == idelClip) return;
        enemySound.clip = idelClip;
        enemySound.Play();
    }

    public void PlayWalkingSound()
    {
        if (enemySound.clip == walkingClip) return;
        enemySound.clip = walkingClip;
        enemySound.Play();
    }

    public void PlayRunningSound()
    {
        if (enemySound.clip == runningClip) return;
        enemySound.clip = runningClip;
        enemySound.Play();
    }

    public void PlayAttackSound()
    {
        if (enemySound.clip == attackClip) return;
        enemySound.clip = attackClip;
        enemySound.Play();
    }

    public void PlayAlertSound()
    {
        if (enemySound.clip == alertClip) return;
        enemySound.clip = alertClip;
        enemySound.Play();
    }

}
