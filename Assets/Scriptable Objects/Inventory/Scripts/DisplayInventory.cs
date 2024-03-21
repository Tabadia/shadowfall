using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    public GameObject inventoryPrefab;
    // GenericPropertyJSON:{"name":"Containter","type":-1,"arraySize":6,"arrayType":"InventorySpace","children":[{"name":"Array","type":-1,"arraySize":6,"arrayType":"InventorySpace","children":[{"name":"size","type":12,"val":6},{"name":"data","type":-1,"children":[{"name":"item","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"47d491d7fc0636646a0e8ddfd3b61a11\",\"localId\":11400000,\"type\":2,\"instanceID\":-35542}"},{"name":"amount","type":0,"val":5},{"name":"backpackSpaceX","type":2,"val":0},{"name":"backpackSpaceY","type":2,"val":0}]},{"name":"data","type":-1,"children":[{"name":"item","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"b4b3ee0260aee674e91ccaa59de77f5c\",\"localId\":11400000,\"type\":2,\"instanceID\":30296}"},{"name":"amount","type":0,"val":1},{"name":"backpackSpaceX","type":2,"val":0},{"name":"backpackSpaceY","type":2,"val":0}]},{"name":"data","type":-1,"children":[{"name":"item","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"607e3c50491b74b49942410723bf2be2\",\"localId\":11400000,\"type\":2,\"instanceID\":-35638}"},{"name":"amount","type":0,"val":2},{"name":"backpackSpaceX","type":2,"val":0},{"name":"backpackSpaceY","type":2,"val":0}]},{"name":"data","type":-1,"children":[{"name":"item","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"942a29e2ddc2b9f4d92eba73af7bb4a2\",\"localId\":11400000,\"type\":2,\"instanceID\":29836}"},{"name":"amount","type":0,"val":2},{"name":"backpackSpaceX","type":2,"val":0},{"name":"backpackSpaceY","type":2,"val":0}]},{"name":"data","type":-1,"children":[{"name":"item","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"0a20ee3886538454e8cdf014f95bc796\",\"localId\":11400000,\"type\":2,\"instanceID\":29872}"},{"name":"amount","type":0,"val":1},{"name":"backpackSpaceX","type":2,"val":0},{"name":"backpackSpaceY","type":2,"val":0}]},{"name":"data","type":-1,"children":[{"name":"item","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"2f505acedcc56eb4086100999b95733d\",\"localId\":11400000,\"type\":2,\"instanceID\":29952}"},{"name":"amount","type":0,"val":1},{"name":"backpackSpaceX","type":2,"val":0},{"name":"backpackSpaceY","type":2,"val":0}]}]}]}
    public InventoryObject inventory;
    public float X_START;
    public float Y_START;
    public float X_SPACE_BETWEEN_ITEM;
    public float NUMBER_OF_COLUMN;
    public float Y_SPACE_BETWEEN_ITEM;
    Dictionary<InventorySpace, GameObject> itemsDisplayed = new Dictionary<InventorySpace, GameObject>();
    // Start is called before the first frame update
    /*void Start()
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
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
            itemsDisplayed.Add(inventory.Container[i], obj);
        }
    }
    public Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEM * (i % NUMBER_OF_COLUMN)), Y_START + (-Y_SPACE_BETWEEN_ITEM   * (int) (i / NUMBER_OF_COLUMN)), 0f);
    }

    public void UpdateDisplay()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            if (itemsDisplayed.ContainsKey(inventory.Container[i]))
            {
                itemsDisplayed[inventory.Container[i]].GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
            }
            else
            {
                var obj = Instantiate(inventory.Container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
                itemsDisplayed.Add(inventory.Container[i], obj);
            }
        }
    }*/
}
