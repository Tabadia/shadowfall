 using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using JetBrains.Annotations;


[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject {
    public string savePath;
    public ItemDatabaseObejct database;
    public Inventory Container;

    public void AddItem(Item item, int amount)
    {
        if (item.buffs.Length > 0)
        {
            SetEmptySpace(item, amount);
            return;
        }

        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].ID == item.Id)
            {
                Container.Items[i].AddAmount(amount);
                return;
            }
        }
        SetEmptySpace(item, amount);
    }
    public InventorySpace SetEmptySpace(Item item, int amount)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].ID <= -1)
            {
                Container.Items[i].UpdateSpace(item.Id, item, amount);
                return Container.Items[i];
            }
        }
        //set up full inv
        return null;
    }
    [ContextMenu("Save")]

    public void Save()
    {
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(file, saveData);
        file.Close();
    }
    [ContextMenu("Load")]

    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();
        }
    }
    [ContextMenu("Clear")]
    public void Clear()
    {
        Container = new Inventory();
    }

    public void MoveItem(InventorySpace item1, InventorySpace item2)
    {
        InventorySpace temp = new InventorySpace(item2.ID, item2.item, item2.amount);
        item2.UpdateSpace(item1.ID, item1.item, item1.amount);
        item1.UpdateSpace(temp.ID, temp.item, temp.amount);
    }

    public void RemoveItem(Item item)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].item == item)
            {
                Container.Items[i].UpdateSpace(-1, null, 0);
            }
        }
    }

    public void DropItem(Item item)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].item == item)
            {
                Container.Items[i].UpdateSpace(-1, null, 0);
            }
        }
    }
}

[System.Serializable]
public class Inventory
{
    public InventorySpace[] Items = new InventorySpace[25]; 
}

[System.Serializable]
public class InventorySpace
{
    public UserInterface parent;
    public int ID;
    public Item item;
    public int amount;
    public InventorySpace()
    {
        this.ID = -1;
        this.item = null;
        this.amount = 0;
    }
    public InventorySpace(int ID, Item item, int amount)
    {
        this.ID = ID;
        this.item = item;
        this.amount = amount;
    }
    public void UpdateSpace(int ID, Item item, int amount)
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
