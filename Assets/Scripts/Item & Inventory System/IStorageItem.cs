using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStorageItem : IInteractable
{
    /* 
     * Specific Instruction to search this StorageItem which stores things.
     */
    public abstract InventorySystem search();

    /*
     * Specific Instruction to equip this StorageItem onto the Player.
     */
    public abstract bool equip(PlayerInformation InPlayerInfo);

    /*
     * Specific Instruction to unequip this StorageItem onto the Player.
     */
    public abstract void unequip(PlayerInformation InPlayerInfo);

    /*
     * The audio that plays during the usage of this Item.
     */
    AudioSource useSound { get; set; }

    /*
     * The amount of Storage that this Item has.
     */
    Vector2 storageSize { get; set; }

    public InventorySystem GetInventorySystem();
}
