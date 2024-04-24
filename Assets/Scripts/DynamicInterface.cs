using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicInterface: UserInterface
{
    public GameObject inventoryPrefab;
    public float X_START;
    public float Y_START;
    public float X_SPACE_BETWEEN_ITEM;
    public float NUMBER_OF_COLUMN;
    public float Y_SPACE_BETWEEN_ITEM;
    public override void CreateSpace()
    {
        spacesOnInterface = new Dictionary<GameObject, InventorySpace>();
        for (int i = 0; i < inventory.Container.Items.Length; i++)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });


            spacesOnInterface.Add(obj, inventory.Container.Items[i]);
        }
    }
    private Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEM * (i % NUMBER_OF_COLUMN)), Y_START + (-Y_SPACE_BETWEEN_ITEM * (int)(i / NUMBER_OF_COLUMN)), 0f);
    }
}
