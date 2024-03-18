using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject, ISerializationCallbackReceiver {
    public ItemDatabaseObejct database;
    public List<InventorySpace> Container = new List<InventorySpace>();
    public void AddItem(ItemObject item, int amount)
    {
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == item)
            {
                Container[i].AddAmount(amount);
                return;
            }
        }
        Container.Add(new InventorySpace(database.GetId[item], item, amount));     
    }

    public void Save()
    {

    }

    public void Load()
    {

    }

    public void OnAfterDeserialize()
    {
        for (int i = 0;i < Container.Count;i++)
        {
            Container[i].item = database.GetItem[Container[i].ID];
        }
    }

    public void OnBeforeSerialize()
    {
    }
}

[System.Serializable]
public class InventorySpace
{
    public int ID;
    public ItemObject item;
    public int amount;
    public float backpackSpaceX;
    public float backpackSpaceY;
    public InventorySpace(int ID, ItemObject item, int amount)
    {
        this.ID = ID;
        this.item = item;
        this.amount = amount;
    }
    public void AddAmount(int value)
    {
        amount += value;
    }
}
