using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]
public class ItemDatabaseObejct : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] ItemsObject;
    [ContextMenu("Update ID's")]
    public void UpdateID()
    {
        for (int i = 0; i < ItemsObject.Length; i++)
        {
            if (ItemsObject[i].data.Id != i)
                ItemsObject[i].data.Id = i;
        }
    }
    public void OnAfterDeserialize() {
        UpdateID();
    }

    public void OnBeforeSerialize()
    {
    }
}
