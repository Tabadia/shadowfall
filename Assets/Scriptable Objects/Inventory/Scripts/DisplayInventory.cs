using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayInventory : MonoBehaviour
{   
    public InventoryObject inventory;
    public int X_START;
    public int Y_START;
    public int X_SPACE_BETWEEN_ITEM;
    public int NUMBER_OF_COLUMN;
    public int Y_SPACE_BETWEEN_ITEM;
    Dictionary<InventorySpace, GameObject> itemsDisplayed = new Dictionary<InventorySpace, GameObject>();
    // Start is called before the first frame update
    void Start()
    {               
        CreateDisplay();    
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    public void CreateDisplay()
    {
        for (int i = 0; i < inventory.Containter.Count; i++)
        {
            var obj = Instantiate(inventory.Containter[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Containter[i].amount.ToString("n0");
            itemsDisplayed.Add(inventory.Containter[i], obj);
        }
    }
    public Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEM * (i % NUMBER_OF_COLUMN)), Y_START + (-Y_SPACE_BETWEEN_ITEM   * (i / NUMBER_OF_COLUMN)), 0f);
    }

    public void UpdateDisplay()
    {
        for (int i = 0; i < inventory.Containter.Count; i++)
        {
            if (itemsDisplayed.ContainsKey(inventory.Containter[i]))
            {
                itemsDisplayed[inventory.Containter[i]].GetComponentInChildren<TextMeshProUGUI>().text = inventory.Containter[i].amount.ToString("n0");
            }
            else
            {
                var obj = Instantiate(inventory.Containter[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Containter[i].amount.ToString("n0");
                itemsDisplayed.Add(inventory.Containter[i], obj);
            }
        }
    }
}
