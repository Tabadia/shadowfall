using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayInventory : MonoBehaviour
{   
    public InventoryObject inventory;

    public int X_SPACE_BETWEEN_ITEM;
    public int NUMBER_OF_COLUMN;
    public int Y_SPACE_BETWEEN_ITEM;
    Dictionary<InventorySpace, GameObject> itemsDisplayed = new Dictionary<InventorySpace, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateDisplay()
    {
        for (int i = 0; i < inventory.Containter.Count; i++)
        {
            var obj = Instantiate(inventory.Containter[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<Transform>().localPosition = new Vector3(0, 0, 0);
        }
    }
    public Vector3 GetPosition(int i)
    {
        return new Vector3(X_SPACE_BETWEEN_ITEM*(i % NUMBER_OF_COLUMN), (-Y_SPACE_BETWEEN_ITEM * (i/NUMBER_OF_COLUMN)), 0f);
    }
}
