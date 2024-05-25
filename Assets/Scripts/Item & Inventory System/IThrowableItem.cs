using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public interface IThrowableItem : IInteractable
{

    /*
     * Specific Instruction to throw this ThrowableItem.
     */
    public abstract bool throwItem(Vector3 forwardUnitVector);

    /*
     * The numberical value that determines the effect of this ThrowableItem
     */
    public abstract float damageValue { get; set; }

    /*
     * The audio that plays when this ThrowableItem Breaks. Checked onCollision of another GameObject's collider
     */
    AudioClip breakSound { get; set; }

}
