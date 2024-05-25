using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConsumableItem : IInteractable
{
    /*
     * Specific Instruction for ConsumableItems to consume it.
     */
    public abstract bool consume();

    /*
     * The numberical value that determines the effect of this ConsumableItem
     */
    float consumableValue { get; set; }

    /*
     * The audio that plays during the usage of this Item.
     */
    AudioSource useSound { get; set; }
}
