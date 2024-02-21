using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<InventorySpace> Containter = new List<InventorySpace>();
    public void AddItem(ItemObject item, int amount)
    {
        bool hasItem = false;
        for (int i = 0; i < Containter.Count; i++)
        {
            if (Containter[i].item == item)
            {
                Containter[i].AddAmount(amount);
                hasItem = true;
                break;
            }
        }
        if(!hasItem)
        {
            Containter.Add(new InventorySpace(item, amount));
        }
    }
}

[System.Serializable]
public class InventorySpace
{
    public ItemObject item;
    public int amount;
    public float backpackSpaceX;
    public float backpackSpaceY;
    public InventorySpace(ItemObject item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }
    public void AddAmount(int value)
    {
        amount += value;
    }
}
