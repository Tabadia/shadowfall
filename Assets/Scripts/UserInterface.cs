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
    public Dictionary<GameObject, InventorySpace> itemsDisplayed = new Dictionary<GameObject, InventorySpace>();
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
        player.mouseItem.hoverObj = obj;
        if (itemsDisplayed.ContainsKey(obj))
        {
            player.mouseItem.hoverItem = itemsDisplayed[obj];
        }
    }
    public void OnExit(GameObject obj)
    {
        player.mouseItem.hoverObj = null;
        player.mouseItem.hoverItem = null;
    }
    public void OnEnterInterface(GameObject obj)
    {
        player.mouseItem.UI = obj.GetComponent<UserInterface>();
    }
    public void OnExitInterface(GameObject obj)
    {
        player.mouseItem.UI = null;
    }
    public void OnDragStart(GameObject obj)
    {
        var mouseObject = new GameObject();
        var rt = mouseObject.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(50, 50);
        mouseObject.transform.SetParent(transform.parent);
        if (itemsDisplayed[obj].ID >= 0)
        {
            var img = mouseObject.AddComponent<Image>();
            img.sprite = inventory.database.GetItem[itemsDisplayed[obj].ID].uiDisplay;
            img.raycastTarget = false;
        }
        player.mouseItem.obj = mouseObject;
        player.mouseItem.item = itemsDisplayed[obj];
    }
    public void OnDragEnd(GameObject obj)
    {
        var itemOnMouse = player.mouseItem;
        var mouseHoverItem = itemOnMouse.hoverItem;
        var mouseHovorObj = itemOnMouse.hoverObj;
        var GetItemObject = inventory.database.GetItem;

        if (itemOnMouse.UI != null)
        {
            if (mouseHovorObj)
            {
                if (mouseHoverItem.CanPlaceInSpace(GetItemObject[itemsDisplayed[obj].ID]) && (mouseHoverItem.item.Id <= -1 || (mouseHoverItem.item.Id >= 0 && itemsDisplayed[obj].CanPlaceInSpace(GetItemObject[mouseHoverItem.item.Id]))))
                {
                    inventory.MoveItem(itemsDisplayed[obj], mouseHoverItem.parent.itemsDisplayed[itemOnMouse.hoverObj]);
                }
            }
        }
        else
        {
            inventory.DropItem(itemsDisplayed[obj].item);
        }
        Destroy(itemOnMouse.obj);
        itemOnMouse.item = null;
    }
    public void OnDrag(GameObject obj)
    {
        if (player.mouseItem.obj != null)
        {
            player.mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }
}

public class MouseItem
{
    public UserInterface UI;
    public GameObject obj;
    public InventorySpace item;
    public InventorySpace hoverItem;
    public GameObject hoverObj;
}