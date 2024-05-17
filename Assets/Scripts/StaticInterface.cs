using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class StaticInterface : UserInterface
{
    public GameObject[] spaces;
    public override void CreateSpace()
    {
        spacesOnInterface = new Dictionary<GameObject, InventorySpace>();
        for (int i = 0; i < inventory.Container.Spaces.Length; i++)
        {
            var obj = spaces[i];
            AddEvent(obj, EventTriggerType.PointerDown, delegate { OnPointerDown(obj); });
            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); }); 
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });
            
            inventory.GetSpaces[i].spaceDisplay = obj;

            spacesOnInterface.Add(obj, inventory.Container.Spaces[i]);
        }
    }
}
