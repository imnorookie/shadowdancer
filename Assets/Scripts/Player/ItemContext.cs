using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContext : MonoBehaviour
{

    private IInteractable _Interactable;


    public ItemContext()
    {
        _Interactable = null;
    }

    public void setInteractable(IInteractable InInteractable)
    {
        _Interactable = InInteractable;
    }

    public IInteractable getInteractable()
    {
        return _Interactable;
    }

    public void executeInteract()
    {
        _Interactable.interact();
    }

    public bool executePickUp(PlayerInformation InPlayerInfo, GameObject item)
    {
        return _Interactable.pickUpItem(InPlayerInfo, item);
    }

    public bool executeEquip(PlayerInformation InPlayerInfo)
    {
        IStorageItem storageItem = (IStorageItem) _Interactable;
        return storageItem.equip(InPlayerInfo);
    }

    public bool executeThrow(Vector3 forwardUnitVector)
    {
        IThrowableItem throwingItem = (IThrowableItem)_Interactable;
        return throwingItem.throwItem(forwardUnitVector);
    }

    // TODO: Implement Feature..
    public void executeSearch(PlayerInformation InPlayerInfo)
    {
        IStorageItem throwingItem = (IStorageItem)_Interactable;
        throwingItem.search();
    }

    public bool executeConsume()
    {
        IConsumableItem throwingItem = (IConsumableItem)_Interactable;
        return throwingItem.consume();
    }

}
