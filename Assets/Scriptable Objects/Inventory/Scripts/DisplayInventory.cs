using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    public GameObject inventoryPrefab;
    public InventoryObject inventory;
    public float X_START;
    public float Y_START;
    public float X_SPACE_BETWEEN_ITEM;
    public float NUMBER_OF_COLUMN;
    public float Y_SPACE_BETWEEN_ITEM;
    Dictionary<GameObject, InventorySpace> itemsDisplayed = new Dictionary<GameObject, InventorySpace>();
    // Start is called before the first frame update
    void Start()
    {               
        CreateSpace();    
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpace();
    }

    public void CreateSpace()
    {
        itemsDisplayed = new Dictionary<GameObject, InventorySpace>();
        for (int i = 0; i < inventory.Container.Items.Length; i++)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            itemsDisplayed.Add(obj, inventory.Container.Items[i]);
        }
    }
    public Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEM * (i % NUMBER_OF_COLUMN)), Y_START + (-Y_SPACE_BETWEEN_ITEM   * (int) (i / NUMBER_OF_COLUMN)), 0f);
    }

    public void UpdateSpace()
    {
        foreach (KeyValuePair<GameObject, InventorySpace> space in itemsDisplayed)
        {
            if (space.Value.ID >= 0)
            {
                space.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[space.Value.item.Id].uiDisplay;
                space.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                space.Key.GetComponentInChildren<TextMeshProUGUI>().text = space.Value.amount == 1 ? "" : space.Value.amount.ToString("n0");
            }
            else
            {
                space.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;  
                space.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                space.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }
}
