using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum ItemType
{
    Food, 
    Equipment,
    Resources,
    Default
}

public enum Attributes
{
    sunlightProtection
}
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Items/item")]
public class ItemObject : ScriptableObject
{
    public Sprite uiDisplay;
    public bool stackable;
    public ItemType type;
    [TextArea(15, 20)]
    public string description;
    public Item data = new Item();

    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }
}

[System.Serializable]
public class Item
{
    public string Name;
    public int Id = -1;
    public ItemBuff[] buffs;
    public GameObject obj;
    public Item()
    {
        Name = "";
        Id = -1;
        obj = null;
    }
    public Item(ItemObject item)
    {
        Name = item.name;
        Id = item.data.Id;
        //string[] GUID = AssetDatabase.FindAssets(ItemDatabaseObejct.prefabNames[Id]);
        //if (GUID.Length <= 0)
        ///{
        ///    Debug.Log("COULD NOT FIND THE ASSET FOR -> " + ItemDatabaseObejct.prefabNames[Id]);
        //}
        ///string PATH = AssetDatabase.GUIDToAssetPath(GUID[0]);
        //obj = AssetDatabase.LoadAssetAtPath<GameObject>(PATH);
        buffs = new ItemBuff[item.data.buffs.Length];
        for(int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(item.data.buffs[i].min, item.data.buffs[i].max)
            {
                attribute = item.data.buffs[i].attribute
            };
        }
    }
}

[System.Serializable]
public class ItemBuff
{
    public Attributes attribute;
    public int value;
    public int min;
    public int max;
    public ItemBuff(int min, int max)
    {
        this.min = min; this.max = max;
        GenerateValue();
    }
    public void GenerateValue()
    {
        value = UnityEngine.Random.Range(min, max);
    }
}