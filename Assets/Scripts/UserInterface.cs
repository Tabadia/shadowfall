using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public abstract class UserInterface : MonoBehaviour
{
    public Player player;
    public InventoryObject inventory;
    public Dictionary<GameObject, InventorySpace> spacesOnInterface = new Dictionary<GameObject, InventorySpace>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < inventory.Container.Items.Length; i++)
        {
            inventory.Container.Items[i].parent = this;
        }
        CreateSpace();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpace();
    }

    public abstract void CreateSpace();
    public void UpdateSpace()
    {
        foreach (KeyValuePair<GameObject, InventorySpace> space in spacesOnInterface)
        {
            if (space.Value.item.Id >= 0)
            {
                space.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = space.Value.ItemObject.uiDisplay;
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
    

    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj)
    {
        MouseData.slotHoveredOver = obj;
    }
    public void OnExit(GameObject obj)
    {
        MouseData.slotHoveredOver = null;
    }
    public void OnEnterInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = obj.GetComponent<UserInterface>();
    }
    public void OnExitInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = null;
    }
    public void OnDragStart(GameObject obj)
    {
        var mouseObject = new GameObject();
        var rt = mouseObject.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(50, 50);
        mouseObject.transform.SetParent(transform.parent);
        if (spacesOnInterface[obj].item.Id >= 0)
        {
            var img = mouseObject.AddComponent<Image>();
            img.sprite = spacesOnInterface[obj].ItemObject.uiDisplay;
            img.raycastTarget = false;
        }
        MouseData.tempItemBeingDragged = mouseObject;
    }
    public void OnDragEnd(GameObject obj)
    {
        Destroy(MouseData.tempItemBeingDragged);
        if(MouseData.interfaceMouseIsOver == null)
        {
            spacesOnInterface[obj].RemoveItem();
            return;
        }
        if (MouseData.slotHoveredOver)
        {
            InventorySpace mouseHoverSpaceData = MouseData.interfaceMouseIsOver.spacesOnInterface[MouseData.slotHoveredOver];
            inventory.SwapItems(spacesOnInterface[obj], mouseHoverSpaceData);
        }
    }
    public void OnDrag(GameObject obj)
    {
        if (MouseData.tempItemBeingDragged != null)
        {
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }   
}

public static class MouseData
{
    public static UserInterface interfaceMouseIsOver;
    public static GameObject tempItemBeingDragged;
    public static GameObject slotHoveredOver;
}