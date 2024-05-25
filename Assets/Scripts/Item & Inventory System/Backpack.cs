using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack : MonoBehaviour, IStorageItem
{

    [SerializeField]
    public Vector2 _InventoryShape;

    [SerializeField]
    public AudioSource _CollisionSound;

    [SerializeField]
    public Vector2 _StorageSize;

    [SerializeField]
    public AudioSource _UseSound;

    private InventorySystem _InventorySystem;

    [SerializeField]
    public Color _InventoryColor;

    public Backpack()
    {
        _InventorySystem = new InventorySystem(_StorageSize);
    }

    public Vector2 storageSize
    {
        get
        {
            return _StorageSize;
        }
        set
        {
            _StorageSize = value;
        }
    }

    public Vector2 inventoryShape
    {
        get
        {
            return _InventoryShape;
        }
        set
        {
            _InventoryShape = value;
        }
    }

    public AudioSource collisionSound
    {
        get
        {
            return _CollisionSound;
        }
        set
        {
            _CollisionSound = value;
        }
    }

    public AudioSource useSound
    {
        get
        {
            return _UseSound;
        }
        set
        {
            _UseSound = value;
        }
    }

    public Color inventoryColor
    {
        get
        {
            return _InventoryColor;
        }
        set
        {
            _InventoryColor = value;
        }
    }

    public void interact()
    {
        Debug.Log("Oh hey, a backpack.");
    }

    public InventorySystem search()
    {
        return _InventorySystem;
    }

    public bool equip(PlayerInformation InPlayerInfo)
    {

        /* 
         * Tentatively switch out GameManagers' ItemSystem so it can render it when press 'I'. Consider switching Player's ItemSystem later on.
         * 1. Set Player's IStorageItem to THIS IStorageItem.
         * 2. Set GameManager.ItemSystem to THIS IStorageItem.ItemSystem.
         */

        // Set Player Storage Item
        InPlayerInfo.SetStorageItem(GetComponent<Backpack>());

        // Set GameManager InventorySystem
        GameManager.Instance.setInventorySystem(this._InventorySystem);

        return true;
    }

    public void unequip(PlayerInformation InPlayerInfo)
    {
        // Set Player Storage Item
        InPlayerInfo.SetStorageItem(null);

        // Set GameManager InventorySystem
        GameManager.Instance.setInventorySystem(null);
    }

    public InventorySystem GetInventorySystem()
    {
        return _InventorySystem;
    }

    // Start is called before the first frame update
    public void Start()
    {
        
        //Debug.Log("Rows: " + _StorageSize.x + " Column: " + _StorageSize.y);
        // OnStart, instantiate the InventorySystem with _StorageSize values.
        _InventorySystem = new InventorySystem(_StorageSize);
        
        // DEBUGGING: OnStart, add "this" GameObject to the GameManager for BookKeeping. We set GameManager onEquip.
        //GameManager.Instance.setInventorySystem(_InventorySystem);

    }

    // Update is called once per frame
    public void Update()
    {
        
    }
}
