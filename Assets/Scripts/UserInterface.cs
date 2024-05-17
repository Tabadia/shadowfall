using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Unity.VisualScripting;

public abstract class UserInterface : MonoBehaviour
{
    public Player player;
    public InventoryObject inventory;
    public Dictionary<GameObject, InventorySpace> spacesOnInterface = new Dictionary<GameObject, InventorySpace>();

    void Start()
    {
        for (int i = 0; i < inventory.Container.Spaces.Length; i++)
        {
            inventory.Container.Spaces[i].parent = this;
        }
        CreateSpace();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
        
    }

    void Update()
    {
        spacesOnInterface.UpdateSpaceDisplay();
    }

    public abstract void CreateSpace();

    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = obj.gameObject.AddComponent<EventTrigger>();

        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj)
    {

        //Debug.Log("OnEnter");
        MouseData.slotHoveredOver = obj;
    }

    public void OnExit(GameObject obj)
    {

        //Debug.Log("On Exit");
        MouseData.slotHoveredOver = null;
    }

    public void OnEnterInterface(GameObject obj)
    {

        //Debug.Log("Enter Interface");
        MouseData.interfaceMouseIsOver = obj.GetComponent<UserInterface>();
    }

    public void OnExitInterface(GameObject obj)
    {
        //Debug.Log("OnExitInterface");
        MouseData.interfaceMouseIsOver = null;
    }

    public void OnDragStart(GameObject obj)
    {
        
        Debug.Log("ON DRAG START OCCURED");
        MouseData.tempItemBeingDragged = CreateTempItem(obj);
    }

    public void OnPointerDown(GameObject obj)
    {
        Debug.Log("Pointer Click Ocurred");
        if (Input.GetMouseButtonDown(1)) // Right-click
        {
            ConsumeItem(obj);
            Debug.Log("ITEM CONSUMED");
            return;
        }
    }

    public GameObject CreateTempItem(GameObject obj)
    {
        GameObject tempItem = null;
        if (spacesOnInterface[obj].item.Id >= 0)
        {
            tempItem = new GameObject();
            var rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(50, 50);
            tempItem.transform.SetParent(transform.parent);
            var img = tempItem.AddComponent<Image>();
            img.sprite = spacesOnInterface[obj].ItemObject.uiDisplay;
            img.raycastTarget = false;
        }
        return tempItem;
    }

    public void OnDragEnd(GameObject obj)
    {
        Destroy(MouseData.tempItemBeingDragged);
        if (MouseData.interfaceMouseIsOver == null)
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

        Debug.Log("ON DRAG");
        if (MouseData.tempItemBeingDragged != null)
        {
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

    private void ConsumeItem(GameObject obj)
    {
        InventorySpace space = spacesOnInterface[obj];
        if (space.item.Id >= 0 && space.ItemObject.type == ItemType.Food)
        {
            ApplyBuffs(space.ItemObject);
            space.AddAmount(-1);
            if (space.amount <= 0)
            {
                space.RemoveItem();
            }
        }
    }

    private void ApplyBuffs(ItemObject itemObject)
    {
        foreach (var buff in itemObject.data.buffs)
        {
            switch (buff.attribute)
            {
                case Attributes.Health:
                    player.currentHealth += buff.value;
                    break;
                case Attributes.Hunger:
                    player.currentHunger += buff.value;
                    break;
                default:
                    break;
            }
        }
    }
}


public static class MouseData
{
    public static UserInterface interfaceMouseIsOver;
    public static GameObject tempItemBeingDragged;
    public static GameObject slotHoveredOver;
}

public static class ExtensionMethods
{
    public static void UpdateSpaceDisplay(this Dictionary<GameObject, InventorySpace> spacesOnInterface)
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
}