using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerExitScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private AudioClip exitClip;

    public static bool isGameEnded = false;
    private void Awake()
    {
        isGameEnded = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            isGameEnded = true;
            Debug.Log("Player has exited the screen");
            canvasGroup.DOFade(1, 2);

            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;

            AudioSource[] audios = FindObjectsOfType<AudioSource>();

            foreach (AudioSource audio in audios)
            {
                audio.Pause();
            }

            AudioSource.PlayClipAtPoint(exitClip, transform.position);
        }
    }
}
