using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public InventoryObject inventory;
    public InventoryObject equipment;

    public Attribute[] attributes;

    public int maxHealth = 100;
    public int maxHunger = 100;

    public float currentHealth;
    public float currentHunger;

    public float stillHungerRate = 1f;
    public float walkHungerRate = 1.5f;
    public float sprintHungerRate = 2f;
    public float hungerRate = 1f;
    public float healthLostFromHungerRate = 2;
    public int timeAlive = 0;

    public HealthHunger healthHunger;

    public bool dead = false;


    // Start is called before the first frame update

    private void OnApplicationQuit()
    {
        inventory.Container.Spaces = new InventorySpace[25];
    }

    void Awake()
    {
        healthHunger.SetMaxHunger(maxHunger);
        healthHunger.SetMaxHealth(maxHealth);
        currentHunger = maxHunger;
        currentHealth = maxHealth;



        inventory.Load();
        equipment.Load();


        if (PlayerPrefs.GetInt("NewGame") == 1)
        {
            inventory.Clear();
            equipment.Clear();
            print("Cleared!");
            PlayerPrefs.SetInt("NewGame", 0);
        }

        for (int i = 0; i < attributes.Length; i++)
        {
            attributes[i].SetParent(this);
        }
        for (int i = 0; i < equipment.GetSpaces.Length; i++)
        {
            equipment.GetSpaces[i].OnBeforeUpdate += OnBeforeSlotUpdate;
            equipment.GetSpaces[i].OnAfterUpdate += OnAfterSlotUpdate;
        }
    }
    public void OnBeforeSlotUpdate(InventorySpace sapce)
    {
        if (sapce.ItemObject == null)
            return;
        switch (sapce.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Removed ", sapce.ItemObject, " on ", sapce.parent.inventory.type, ", Allowed Items: ", string.Join(", ", sapce.AllowedItems)));

                for (int i = 0; i < sapce.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == sapce.item.buffs[i].attribute)
                            attributes[j].value.RemoveModifier(sapce.item.buffs[i]);
                    }
                }

                break;

            default:
                break;
        }
    }
    public void OnAfterSlotUpdate(InventorySpace space)
    {
        if (space.ItemObject == null)
            return;
        switch (space.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Placed ", space.ItemObject, " on ", space.parent.inventory.type, ", Allowed Items: ", string.Join(", ", space.AllowedItems)));

                for (int i = 0; i < space.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == space.item.buffs[i].attribute)
                            attributes[j].value.AddModifier(space.item.buffs[i]);
                    }
                }

                break;
            default:
                break;
        }
    }
    [System.Serializable]
    public class Attribute
    {
        [System.NonSerialized]
        public Player parent;
        public Attributes type;
        public ModifiableInt value;

        public void SetParent(Player _parent)
        {
            parent = _parent;
            value = new ModifiableInt(AttributeModified);
        }
        public void AttributeModified()
        {
            parent.AttributeModified(this);
        }
    }
    public void AttributeModified(Attribute attribute)
    {
        Debug.Log(string.Concat(attribute.type, " was updated! Value is now ", attribute.value.ModifiedValue));
    }

    // Update is called once per frame
    void Update()
    {
        if((currentHealth <= 0)){
            if (!dead){
                dead = true;
                //print("You died");
            }
        }


        if (currentHealth > 0)
        {
            dead = false;
        }

            currentHunger = Mathf.Max(0, currentHunger - hungerRate * Time.deltaTime);
            healthHunger.SetHunger(currentHunger);

            if (currentHunger <= 0)
                currentHealth = Mathf.Max(0, currentHealth - healthLostFromHungerRate * Time.deltaTime);
            healthHunger.SetHealth(currentHealth);

        inventory.Save();
        equipment.Save();
    }



    public void SprintHunger(){
        hungerRate = sprintHungerRate;
    }

    public void WalkHunger(){
        hungerRate = walkHungerRate;
    }

    public void StillHunger(){
        hungerRate = stillHungerRate;
    }

    public void setHealth(float health)
    {
        currentHealth = health;
    }
    
    public void setHunger(float hunger)
    {
        currentHunger = hunger;
    }
}
