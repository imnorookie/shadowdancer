using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;

public class GameManager : MonoBehaviour
{
    public const int SPACING = 5;
    public const int RELATIVE_CANVAS_HEIGHT_LOCATION = 15;
    public const int RELATIVE_CANVAS_WIDTH_LOCATION = -30;

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    private List<Light> _Lights = new List<Light>();

    // Inventory System Member Variable. Referenced by Player and Canvas. Consider replacing later on with a reference to the Player to directly refernece their StorageItem.
    private InventorySystem _InventorySystem = null;

    private GameObject[,] _UIPrefabs;

    [SerializeField]
    private Canvas _Canvas;

    //[SerializeField]
    //public RectTransform _Panel;

    [SerializeField]
    public Image _Image;

    private bool toggleInventoryRender = false;

    //[SerializeField]
    //public GameObject DEBUGITEMINVENTORY1; // DEBUG PURPOSES ONLY
    //[SerializeField]
    //public GameObject DEBUGITEMINVENTORY2; // DEBUG PURPOSES ONLY
    //[SerializeField]
    //public GameObject DEBUGITEMINVENTORY3; // DEBUG PURPOSES ONLY


    // Make sure there is only ever one GameManager
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        // Do stuff
        //RELATIVE_CANVAS_HEIGHT_LOCATION += Screen.height % (Screen.height / 4);
        //RELATIVE_CANVAS_WIDTH_LOCATION += Screen.width % (Screen.width / 4);
    }

    // Start is called before the first frame update
    public void Start()
    {
        
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {

            if (haveStorageItem())
            {
                toggleInventoryRender = !toggleInventoryRender;
                switch (toggleInventoryRender)
                {
                    case true:
                        renderInventorySystem();
                        break;
                    case false:
                        cleanUI();
                        break;
                }
            } else
            {
                Debug.Log("I don't have a Storage Item.");
            }

        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            // Debugging Purposes - Testing removing Items by GameObject InstanceID. It works.
            _InventorySystem.removeItem(_InventorySystem._InventoryGrid[0,0]);
        }

    }

    public void addLight(Light inputLight)
    {
        _Lights.Add(inputLight);

        Debug.Log("========== Number of Lights in Level: " + _Lights.Count);
        //printLightsList();

    }

    private void printLightsList()
    {
        for (int i = 0; i < _Lights.Count; i++)
        {
            Debug.Log(_Lights[i].GetInstanceID());
        }
        Debug.Log("====================");
    }

    /*
     * Removes the given gameObject from the _Lights List.
     * Arguments:
     * Light
     */
    public void removeLight(Light inputLight)
    {
        Debug.Log("Removing LightID: " + inputLight.gameObject.GetInstanceID().ToString());
        Destroy(inputLight.gameObject);
        _Lights.Remove(inputLight);
    }

    public void disableLight(Light inputLight)
    { 
        inputLight.gameObject.GetComponent<Light>().enabled = false;
    }

    public List<Light> getLights()
    {
        return _Lights;
    }

    public void setInventorySystem(InventorySystem inputInventorySystem)
    {
        _InventorySystem = inputInventorySystem;
        _UIPrefabs = new GameObject[inputInventorySystem._Rows, inputInventorySystem._Columns];
    }

    public void renderInventorySystem()
    {

        // OnClick 'i', GameManager.Instance.renderInventorySystem(). 
        //Debug.Log("Rows: " + _InventorySystem._Rows + " Column: " + _InventorySystem._Columns);
        for (int i = 0; i < _InventorySystem._Rows;  i++)
        {
            for (int j = 0; j< _InventorySystem._Columns; j++)
            {
                //Debug.Log("FUCKK,"+ _InventorySystem._Rows+", " + _InventorySystem._Columns);

                var inst = Instantiate(_Image, new Vector3(_Canvas.transform.position.x + i * SPACING + RELATIVE_CANVAS_WIDTH_LOCATION, _Canvas.transform.position.y + j * -SPACING + RELATIVE_CANVAS_HEIGHT_LOCATION, _Canvas.transform.position.z), Quaternion.identity);
                if (_InventorySystem._InventoryGrid[i,j] != null)
                {
                    inst.GetComponent<Image>().color = _InventorySystem._InventoryGrid[i, j].GetComponent<IInteractable>().inventoryColor;
                } else
                {
                    inst.GetComponent<Image>().color = Color.grey;
                }
                //inst.transform.parent = _Canvas.transform;
                inst.transform.SetParent(_Canvas.transform);


                if (_InventorySystem._InventoryGrid[i,j] != null)
                {
                    var UIPrefabInst = Instantiate(_InventorySystem._InventoryGrid[i, j], new Vector3(_Canvas.transform.position.x + i * SPACING + RELATIVE_CANVAS_WIDTH_LOCATION, _Canvas.transform.position.y + j * -SPACING + RELATIVE_CANVAS_HEIGHT_LOCATION, _Canvas.transform.position.z), Quaternion.identity);

                    _UIPrefabs[i,j] = UIPrefabInst;
                    UIPrefabInst.SetActive(true);
                    UIPrefabInst.transform.localScale *= 5;
                    UIPrefabInst.layer = LayerMask.NameToLayer("UI");
                    UIPrefabInst.GetComponent<Rigidbody>().useGravity = false;
                } 

            }
        }

    }

    public bool getToggleInventoryRender()
    {
        return toggleInventoryRender;
    }

    public void cleanUI()
    {
        foreach(Transform child in _Canvas.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach(GameObject gameObject in _UIPrefabs)
        {
            if (gameObject != null)
            {
                //Debug.Log("Destroy: " + gameObject.name);
                GameObject.Destroy(gameObject);
            }
        }
    }

    public void refreshUI(bool toggleInventoryRender)
    {
        switch (toggleInventoryRender)
        {
            case true:
                cleanUI();
                renderInventorySystem();
                break;
            case false:
                cleanUI();
                break;
        }
    }

    public bool haveStorageItem()
    {

        return !Object.ReferenceEquals(_InventorySystem, null);

    }

}