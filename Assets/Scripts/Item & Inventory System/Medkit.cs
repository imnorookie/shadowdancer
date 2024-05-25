using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Medkit : MonoBehaviour, IConsumableItem
{

    public DeathManager deathManager;

    public Vector2 _InventoryShape;

    [SerializeField]
    public AudioSource _CollisionSound;

    [SerializeField]
    public AudioSource _UseSound;

    [SerializeField]
    public float _ConsumableValue;

    [SerializeField]
    public Color _InventoryColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);

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

    public float consumableValue
    {
        get
        {
            return _ConsumableValue;
        }
        set
        {
            _ConsumableValue = value;
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
        Debug.Log("A Medkit, that'll be useful.");
    }

    public bool consume()
    {
        // TODO: Implement Health Restoration here
        deathManager.RemoveDamagedScreen();
        Debug.Log("Consume Medkit");
        return true;
    }

    // Start is called before the first frame update
    public void Start()
    {
        // OnStart, add "this" GameObject to the GameManager for BookKeeping
    }

    // Update is called once per frame
    public void Update()
    {
        
    }

}
