using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class DisplayInventory : MonoBehaviour
{
    public MouseItem mouseItem = new MouseItem();
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

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });


            itemsDisplayed.Add(obj, inventory.Container.Items[i]);
        }
    }
    public void UpdateSpace()
    {
        foreach (KeyValuePair<GameObject, InventorySpace> space in itemsDisplayed)
        {
            if (space.Value.ID >= 0)
            {
                Sprite sprite = inventory.database.GetItem[space.Value.item.Id].uiDisplay;
                space.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = sprite;
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
    public Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEM * (i % NUMBER_OF_COLUMN)), Y_START + (-Y_SPACE_BETWEEN_ITEM * (int)(i / NUMBER_OF_COLUMN)), 0f);
    }

    private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj)
    {

    }
    public void OnExit(GameObject obj)
    {

    }
    public void OnDragStart(GameObject obj)
    {
        var mouseObject = new GameObject();
        var rt = mouseObject.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(50, 50);
        mouseObject.transform.SetParent(transform.parent);
        if (itemsDisplayed[obj].ID >= 0 )
        {
            var img = mouseObject.AddComponent<Image>();
            img.sprite = inventory.database.GetItem[itemsDisplayed[obj].ID].uiDisplay;
            img.raycastTarget = false;
        }
        mouseItem.obj = mouseObject;
        mouseItem.item = itemsDisplayed[obj];
    }
    public void OnDragEnd(GameObject obj)
    {
        if (mouseItem.obj != null)
        {
            mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }
    public void OnDrag(GameObject obj)
    {

    }
}

public class MouseItem
{
    public GameObject obj;
    public InventorySpace item;
    public InventorySpace hoverItem;
    public GameObject hoverObj;
}