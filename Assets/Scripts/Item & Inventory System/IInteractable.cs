using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    /*
     * Generic Interact action for all Items. 
     */
    public abstract void interact();

    /* 
     * Generic PickUp Action that all Items should have. Returns Script Component.
     * Possibly redundant
     */
    public bool pickUpItem(PlayerInformation InPlayerInfo, GameObject item)
    {
        IStorageItem tempStorageItem = InPlayerInfo.GetStorageItem();
        if (tempStorageItem == null)
        {
            Debug.Log("No Storage Item.");
        }

        bool temp = InPlayerInfo.GetStorageItem().GetInventorySystem().findFirstFit(item);

        if (!temp)
        {
            Debug.Log("No Space in Storage Item.");
            return false;
        }
        else
        {
            GameManager.Instance.cleanUI();
            GameManager.Instance.setInventorySystem(InPlayerInfo.GetStorageItem().GetInventorySystem());
            return true;
        }
    }

    /*
     * inventoryShape defines the number of slots and how many that an Item will take up. 
     */
    Vector2 inventoryShape { get; set; }

    /*
     * All Items have a collision sound.
     */
    AudioSource collisionSound { get; set; }

    /*
     * The inventory color for this type of Item
     */
    Color inventoryColor { get; set; }

}
