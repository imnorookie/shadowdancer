using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ConcreteBrick : MonoBehaviour, IThrowableItem
{

    public Vector2 _InventoryShape;

    [SerializeField]
    public AudioSource _CollisionSound;

    [SerializeField]
    public float _DamageValue;

    [SerializeField]
    public AudioClip _BreakSound;

    [SerializeField]
    public Color _InventoryColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

    [SerializeField]
    public float _LaunchVelocity = 300;

    public float damageValue 
    {
        get 
        {
            return _DamageValue;
        }
        set
        {
            _DamageValue = value;
        }
    }

    public AudioClip breakSound
    {
        get
        {
            return _BreakSound;
        }
        set
        {
            _BreakSound = value;
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
        Debug.Log("Wow. A concrete brick.");
    }

    public bool throwItem(Vector3 forwardUnitVector)
    {
        // TODO: Throwing Implementation.
        Debug.Log("Throw Concrete Brick");

        this.gameObject.SetActive(true);
        this.GetComponent<Rigidbody>().AddForce(forwardUnitVector * _LaunchVelocity);

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

    public void OnCollisionExit(Collision collision)
    {
        Debug.Log("Concrete Brick Collision Exit!");
        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "PlayerParent")
        {
            // Play the break sound here..
            // ..
            if (collision.gameObject.tag == "Light")
                Destroy(collision.gameObject);

            AudioSource.PlayClipAtPoint(breakSound, transform.position);

            PlayerInformation playerState = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInformation>();

            playerState.SetThrownObjectIntensity(9);
            playerState.SetThrownObjectPosition(transform.position);


            // Debugging
            Debug.Log("Concrete Brick Collision Exit!");

            // Destroy this gameobject.
            Destroy(this.gameObject);
        }
    }

}
