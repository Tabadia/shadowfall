using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Object", menuName = "Inventory System/Items/Equipment")]
public class EquipmentObject : ItemObject
{
    public float sunlightResistanceBonus;
    public void Awake()
    {
        type = ItemType.Equipment;
    }
}
