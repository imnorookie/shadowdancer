using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Unity;

public class InventorySystem : MonoBehaviour
{

    public int _Rows;
    public int _Columns;

    public GameObject[,] _InventoryGrid;

    public InventorySystem(Vector2 inVector2)
    {
        _Rows = (int)inVector2.x;
        _Columns = (int)inVector2.y;
        instantiateInventoryGrid();
        
    }

    public void instantiateInventoryGrid()
    {
        _InventoryGrid = new GameObject[_Rows, _Columns];
    }

    public bool findFirstFit(GameObject inItem)
    {
        for (int i = 0; i < _Rows; i++)
        {
            for(int j = 0; j < _Columns; j++)
            {
                // Only start a check if there is a empty space.
                if (_InventoryGrid[i, j] == null)
                {

                    // Do the check.
                    if (checkFit(new Vector2(i,j), inItem.GetComponent<IInteractable>().inventoryShape))
                    {
                        // There is space. Also passing the original object in, not a copy.
                        setItemPointer(new Vector2(i,j), ref inItem);
                        return true;
                    }
                }
            }
        }

        // No space.
        return false;
    }

    public bool checkFit(Vector2 curGridPos, Vector2 ItemInventoryShape)
    {
        for (int i = (int)curGridPos.x; i < (int)curGridPos.x + ItemInventoryShape.x; i++)
        {
            for (int j = (int)curGridPos.y; j < (int)curGridPos.y + ItemInventoryShape.y; j++)
            {

                if (i < _Rows && j < _Columns) 
                { 
                    if (_InventoryGrid[i, j] != null)
                    {
                        return false;
                    }
                } 
                else
                {
                    return false;
                }

            }
        }
        return true;
    }

    public void setItemPointer(Vector2 curGridPos, ref GameObject inItem)
    {
        for (int i = (int)curGridPos.x; i < (int)curGridPos.x + inItem.GetComponent<IInteractable>().inventoryShape.x; i++)
        {
            for (int j = (int)curGridPos.y; j < (int)curGridPos.y + inItem.GetComponent<IInteractable>().inventoryShape.y; j++)
            {
                _InventoryGrid[i, j] = inItem;
            }
        }
    }

    public List<Vector2> buildRemoveIndexItems(GameObject InItem)
    {
        List<Vector2> list = new List<Vector2>();

        for (int i = 0; i < _Rows; i++)
        {
            for (int j = 0; j < _Columns; j++)
            {
                if (_InventoryGrid[i, j] != null)
                {
                    if (_InventoryGrid[i, j].GetInstanceID() == InItem.GetInstanceID())
                    {
                        list.Add(new Vector2(i, j));
                    }
                }
            }
        }

        return list;
    }

    public bool removeItem(GameObject InItem)
    {
        // First, build indexes that need to be removed.
        List<Vector2> intermediate = buildRemoveIndexItems(InItem);

        // Remove each Item at each Vector2
        for (int listNum = 0; listNum < intermediate.Count; listNum++)
        { 
            _InventoryGrid[(int)intermediate[listNum].x, (int)intermediate[listNum].y] = null;
        }

        // Refresh UI
        GameManager.Instance.refreshUI(GameManager.Instance.getToggleInventoryRender());
        //Destroy(InItem);
        return true;
    }

    public GameObject findItem<T>()
    {
        // Find First Item of given type
        for (int i = 0; i < _Rows; i++)
        {
            for (int j = 0; j < _Columns; j++)
            {
                if (_InventoryGrid[i, j] != null)
                {
                    IInteractable temp = _InventoryGrid[i, j].GetComponent<IInteractable>();
                    // Checking if the GameObject holds an Item of Type T.
                    if (temp is T)
                    {
                        return _InventoryGrid[i, j];
                    }
                }
            }
        }

        // Nothing of given Type was found.
        return null;
    }

    public GameObject getItem_atIndex(int x, int y)
    {
        return _InventoryGrid[x, y];
    }

    // Start is called before the first frame update
    public void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        
    }
}
