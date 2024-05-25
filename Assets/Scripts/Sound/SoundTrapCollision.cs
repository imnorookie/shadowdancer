using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrapCollision : MonoBehaviour
{

    //Player game object
    public PlayerInformation player;

    public int collisionSoundIntensity = 9;

    public float duration = 2f;

    [SerializeField]
    private AudioSource sound;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInformation>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerParent")
        {
            sound.PlayOneShot(sound.clip);
            int temp = player.GetFloorSoundIntensity();
            //Set the sound intensity to the collision sound intensity
            player.SetSoundIntensity(collisionSoundIntensity);
            player.SetFloorSoundIntensity(collisionSoundIntensity);
            StartCoroutine(WaitForTime(temp));
            GetComponent<Rigidbody>().AddForce(collision.contacts[0].normal * 100, ForceMode.Impulse);
        }
    }

    //Make a coroutine to keep setting the sound Intensity for set time
    private IEnumerator WaitForTime(int floorIntensity)
    {
        Debug.Log("Waiting");
        yield return new WaitForSeconds(duration);
        player.SetFloorSoundIntensity(floorIntensity);
    }
}
