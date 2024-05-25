using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpscareTrigger : MonoBehaviour
{
    [SerializeField]
    private AudioSource jumpscareSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //NOTE: This is a temporary solution. The sound should be played at the location of the jumpscare, not at the player.
            //AudioSource.PlayClipAtPoint(jumpscareSound.clip, new Vector3(600,0,500));

            jumpscareSound.PlayOneShot(jumpscareSound.clip);

            //destroy the jumpscare trigger after it has been triggered
            Destroy(gameObject);
            Destroy(jumpscareSound, 5f);
        }
    }
}
