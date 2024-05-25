using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
This class stores all information on the player's state.
I.E. luminance and soundIntensity, while also exposing methods
 to set and get those attributes.
*/
public class PlayerInformation : MonoBehaviour
{

    private float luminance;
    private int soundIntensity;
    private int floorSoundIntensity; // intensity of what the player is moving on

    private int thrownObjectIntensity;

    private Vector3 thrownObjectPosition;

    // Player Item Storage
    private IStorageItem _StorageItem;

    // Item Context to Decouple Player from Item System - NEED to Instantiate it or it all breaks.
    private ItemContext _ItemContext = new ItemContext();

    private float thrownObjectResetTime = 0.5f;
    private float thrownObjectTimer = 0.0f;


    void Start() {
        luminance = 0.0f;
        soundIntensity = 0;
        floorSoundIntensity = 0;
        thrownObjectPosition = Vector3.zero;
    }

    void Update() {
        if (thrownObjectPosition != Vector3.zero) {
            Debug.Log("Made this much sound " + thrownObjectIntensity + " at " + thrownObjectPosition);
            thrownObjectTimer += Time.deltaTime;
            if (thrownObjectTimer >= thrownObjectResetTime) {
                thrownObjectPosition = Vector3.zero;
                thrownObjectIntensity = 0;
                thrownObjectTimer = 0.0f;
            }
        }
    }
    
    public float GetLuminance() {
        return luminance;
    }

    public void SetLuminance(float newLuminance) {
        luminance = newLuminance;
        Debug.Log("Player Illumination Set: " + luminance); 
    }

    public int GetSoundIntensity()
    {
        return soundIntensity;
    }

    public void SetSoundIntensity(int intensity)
    {
        int sanitizedIntensity = Mathf.Clamp(intensity, 0, 10);
        soundIntensity = sanitizedIntensity;
    }

    public int GetFloorSoundIntensity() {
        return floorSoundIntensity;
    }

    public void SetFloorSoundIntensity(int intensity) {
        floorSoundIntensity = intensity;
    }

    public void SetDominatingSoundIntensity(int movementSoundIntensity) {
        if (movementSoundIntensity > floorSoundIntensity)
            SetSoundIntensity(movementSoundIntensity);
        else 
            SetSoundIntensity(floorSoundIntensity);
    }

    public void SetThrownObjectIntensity(int intensity)
    {
        this.thrownObjectIntensity = intensity;
    }

    public int GetThrownObjectIntensity()
    {
        return this.thrownObjectIntensity;
    }

    public void SetThrownObjectPosition(Vector3 position) {
        this.thrownObjectPosition = position;
        this.thrownObjectTimer = 0.0f;
        Debug.Log("Set the timer and position at " + thrownObjectPosition);
    }

    public Vector3 GetThrownObjectPosition() {
        return this.thrownObjectPosition;
    }

    public void SetStorageItem(IStorageItem InStorageItem)
    {
        _StorageItem = InStorageItem;
    }

    public IStorageItem GetStorageItem()
    {
        return _StorageItem;
    }

    public void SetItemContext(IInteractable InInteractable)
    {
        _ItemContext.setInteractable(InInteractable);
    }

    public ItemContext GetItemContext()
    {
        return _ItemContext;
    }
}
