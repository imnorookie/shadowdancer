using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// rough script for storing actions unrelated to movement
public class PlayerActions : MonoBehaviour
{
    [SerializeField] // serializing 4 'E' to interact gamers
    private KeyCode interact;
    // Start is called before the first frame update

    [SerializeField]
    private float interactRange = 2f; // player interact distance

    [SerializeField]
    private static int itemLayerNum = 9;

    private int itemLayerMask = 1 << itemLayerNum;

    public DeathManager deathManager;

    private PlayerInformation playerInformation;

    [SerializeField]
    public Camera _PlayerCamera;

    void Start()
    {
        playerInformation = GetComponent<PlayerInformation>();
        
        // read stuff from a file or something in here for keybaord controls
        // for now hard-coded to F (i'm an F-to-interact gamer, E is for SCRUBS)
        if (interact == KeyCode.None)
            interact = KeyCode.F;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(interact))
        //    interactHandler();
        //Debug.Log(playerInformation.GetSoundIntensity());
        // uncomment below to draw out the ray that the player grabs items with
        // Ray centre_crosshair = Camera.main.ViewportPointToRay(new Vector3 (0.5f, 0.5f, 0));
        // Debug.DrawRay(centre_crosshair.origin, centre_crosshair.direction * interactRange, Color.yellow);

        // Debugging:
        if (Input.GetKeyDown(KeyCode.F))
        {
            interactHandler(KeyCode.F);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            interactHandler(KeyCode.E);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            interactHandler(KeyCode.Q);
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            interactHandler(KeyCode.I);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            interactHandler(KeyCode.P);
        }

    }

    private void itemInteract(GameObject item)
    {
        playerInformation.SetItemContext(item.GetComponent<IInteractable>());
        playerInformation.GetItemContext().executeInteract();
    }

    // General Action: Pick Up 
    private bool itemPickUp(GameObject item)
    {
        playerInformation.SetItemContext(item.GetComponent<IInteractable>());

        if (playerInformation.GetItemContext().executePickUp(playerInformation, item))
        {
            //Destroy(item) will break the system.
            item.SetActive(false);

            // Refresh to get latest Render.
            GameManager.Instance.refreshUI(GameManager.Instance.getToggleInventoryRender());

            return true;
        }

        return false;
    }

    // Storage Action: Equip Action
    private bool itemEquip(GameObject item)
    {
        if (item.GetComponent<IInteractable>() is IStorageItem) 
        {
            // Set the Item Context to that of a IStorageItem
            playerInformation.SetItemContext((IStorageItem)item.GetComponent<IInteractable>());

            // Make sure the Item is a StorageItem then Equip it.
            if (item.GetComponent<IInteractable>() is IStorageItem)
            {
                if (playerInformation.GetItemContext().executeEquip(playerInformation))
                {
                    Destroy(item);
                    return true;
                }
            }
        }

        Debug.Log("Can't equip that.");
        return false;
    }

    private bool itemThrow()
    {
        /* 
         * TODO: Throwing Implementation.
         * Tentatively, find the first Item that is IThrowable, and throw it.
         */

        GameObject ThrownItem = playerInformation.GetStorageItem().GetInventorySystem().findItem<IThrowableItem>();

        if (ThrownItem != null && ThrownItem.GetComponent<IInteractable>() is IThrowableItem)
        {
            Debug.Log(ThrownItem.name);

            // Set ItemContext
            playerInformation.SetItemContext((IThrowableItem)ThrownItem.GetComponent<IInteractable>());

            // Make sure the Item is a IThrowableItem then Throw it.
            if (ThrownItem.GetComponent<IInteractable>() is IThrowableItem)
            {

                ThrownItem.gameObject.transform.position = _PlayerCamera.transform.position;

                // Execute Action: Throw
                if (playerInformation.GetItemContext().executeThrow(_PlayerCamera.transform.forward))
                {
                    // Remove Throwable Item from Inventory.
                    playerInformation.GetStorageItem().GetInventorySystem().removeItem(ThrownItem);

                }
            }

            return true;
        } 
        else
        {
            Debug.Log("I have nothing to Throw.");
            return false;
        }
    }

    private bool itemConsume()
    {
        /* 
         * TODO: Consuming Implementation.
         * Tentatively, find the first Item that is IConsumableItem, and Consume it.
         */

        GameObject ConsumedItem = playerInformation.GetStorageItem().GetInventorySystem().findItem<IConsumableItem>();

        if (ConsumedItem != null && ConsumedItem.GetComponent<IInteractable>() is IConsumableItem)
        {
            Debug.Log(ConsumedItem.name);

            // Set ItemContext
            playerInformation.SetItemContext((IConsumableItem)ConsumedItem.GetComponent<IInteractable>());

            // Make sure the Item is a IThrowableItem then Throw it.
            if (ConsumedItem.GetComponent<IInteractable>() is IConsumableItem)
            {
                // Execute Action: Throw
                if (playerInformation.GetItemContext().executeConsume())
                {
                    // Remove Throwable Item from Inventory.
                    playerInformation.GetStorageItem().GetInventorySystem().removeItem(ConsumedItem);
                }
            }

            return true;
        }
        else
        {
            Debug.Log("I have nothing to Consume.");
            return false;
        }
    }

    private void itemSearch()
    {
        // TODO: Searching Implementation.
    }

    private void interactHandler(KeyCode inKeyCode) {
        Debug.Log("Interact key hit");
        Ray centre_crosshair = Camera.main.ViewportPointToRay(new Vector3 (0.5f, 0.5f, 0));
        RaycastHit hitData;

        /* below raycast for item grabbing purposes FOR NOW, as we add more 
           interactions we will refactor to not raycast just on the one
           layer mask.
        */
        Physics.Raycast(centre_crosshair, out hitData, interactRange, itemLayerMask);
        if (hitData.transform != null)
        //itemInteract(hitData.transform.gameObject);
        {
            switch (inKeyCode)
            {
                case KeyCode.F:
                    itemInteract(hitData.transform.gameObject);
                    break;
                case KeyCode.Q:
                    itemEquip(hitData.transform.gameObject);
                    break;
                case KeyCode.E:
                    itemPickUp(hitData.transform.gameObject);
                    break;
            }
        } 
        else
        {
            switch(inKeyCode)
            {
                case KeyCode.I:
                    itemThrow();
                    break;
                case KeyCode.P:
                    itemConsume();
                    deathManager.RemoveDamagedScreen();
                    break;
            }
        }
    }
}
