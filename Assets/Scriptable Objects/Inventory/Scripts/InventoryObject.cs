
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using Unity.VisualScripting.FullSerializer;
using System.Collections;


[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject {
    public string savePath;
    public ItemDatabaseObejct database;
    public Inventory Container;
    public InventorySpace[] GetSpaces { get { return Container.Spaces; } }
    public Player player;
    GameObject obj;


    public bool AddItem(Item item, int amount)
    {
        if (EmptySpaceCount <= 0)
        {
            return false;
        }
        InventorySpace space = FindItemOnInventory(item);
        if (!database.ItemsObject[item.Id].stackable || item == null)
        {
            SetEmptySpace(item, amount);
            return true;
        }
        space.AddAmount(amount);
        return true;
    }
    public int EmptySpaceCount
    {
        get
        {
            int counter = 0;
            for (int q = 0; q < Container.Spaces.Length; q++)
            {
                if (Container.Spaces[q].item.Id <= -1)
                    counter++;
            }
            return counter;
        }
    }
    public InventorySpace FindItemOnInventory(Item item)
    {
        for (int i = 0; i < Container.Spaces.Length; i++)
        {
            if (Container.Spaces[i].item.Id == item.Id)
            {
                return Container.Spaces[i]; 
            }
        }
        return null;
    }
    public InventorySpace SetEmptySpace(Item item, int amount)
    {
        for (int i = 0; i < Container.Spaces.Length; i++)
        {
            if (Container.Spaces[i].item.Id <= -1)
            {
                Container.Spaces[i].UpdateSpace(item, amount);
                return Container.Spaces[i];
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
        Container.Clear();
    }
    
    public void SwapItems(InventorySpace item1, InventorySpace item2)
    {
        if(item2.CanPlaceInSpace(item1.ItemObject) && item1.CanPlaceInSpace(item2.ItemObject))
        {
            InventorySpace temp = new InventorySpace(item2.item, item2.amount);
            item2.UpdateSpace(item1.item, item1.amount);
            item1.UpdateSpace(temp.item, temp.amount);
        }


    }

    public void RemoveItem(Item item)
    {
        for (int i = 0; i < Container.Spaces.Length; i++)
        {
            if (Container.Spaces[i].item == item)
            {
                Container.Spaces[i].UpdateSpace(null, 0);
            }
        }
    }

    public void DropItem(Item item)
    {
        for (int i = 0; i < Container.Spaces.Length; i++)
        {
            if (Container.Spaces[i].item == item)
            {
                MonoInstance.instance.StartCoroutine(LoadAsset("mapobjects", item.Name));
                Debug.Log(obj);
            }
        }
    }
    IEnumerator LoadAsset(string assetBundleName, string objectNameToLoad)
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "AssetBundles");
        filePath = System.IO.Path.Combine(filePath, assetBundleName);
        Debug.Log(filePath);

        //Load "animals" AssetBundle
        var assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(filePath);
        yield return assetBundleCreateRequest;

        AssetBundle asseBundle = assetBundleCreateRequest.assetBundle;

        //Load the "dog" Asset (Use Texture2D since it's a Texture. Use GameObject if prefab)
        AssetBundleRequest asset = asseBundle.LoadAssetAsync<GameObject>(objectNameToLoad);
        yield return asset;
        Debug.Log(asset);

        //Retrieve the object (Use Texture2D since it's a Texture. Use GameObject if prefab)
        GameObject loadedAsset = asset.asset as GameObject;

        //Do something with the loaded loadedAsset  object (Load to RawImage for example) 
        obj = loadedAsset;
    }
}

public class MonoInstance : MonoBehaviour
{
    public static MonoInstance instance;

    private void Start()
    {
        MonoInstance.instance = this;
    }
}

[System.Serializable]
public class Inventory
{
    public InventorySpace[] Spaces = new InventorySpace[25]; 
    public void Clear()
    {
        for (int i = 0; i < Spaces.Length; i++)
        {
            Spaces[i].RemoveItem();
        }
    }
}

[System.Serializable]
public class InventorySpace
{
    public ItemType[] AllowedItems = new ItemType[0];
    [System.NonSerialized]
    public UserInterface parent;
    [System.NonSerialized]
    public GameObject spaceDisplay;
    public Item item = new Item();
    public int amount;

    public ItemObject ItemObject
    {
        get
        {
            if (item.Id >= 0)
            {
                return parent.inventory.database.ItemsObject[item.Id];
            }
            return null;
        }
    }
    public InventorySpace()
    {
        item = new Item();
        amount = 0;
    }
    public InventorySpace(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }
    public void UpdateSpace(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;

    }
    public void RemoveItem()
    {
        item = new Item();
        amount = 0;
    }
    public void AddAmount(int value)
    {
        amount += value;
    }
    public bool CanPlaceInSpace(ItemObject itemObject)
    {
        if(AllowedItems.Length <= 0 || itemObject == null || itemObject.data.Id < 0)
        {
            return true;
        }
        for (int i = 0; i < AllowedItems.Length; i++)
        {
            if (itemObject.type == AllowedItems[i])
            {
                return true;
            }
        }
        return false;
    }
}
