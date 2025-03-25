using System.Collections.Generic;
using Data;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private string SaveKey = "PlayerInventory";
    
    private Dictionary<string, InventoryItem> items = new Dictionary<string, InventoryItem>();
    
    private void Awake()
    {
        LoadInventory();
    }

    public void AddItem(ItemData item, string UID)
    {
        if (items.ContainsKey(item.itemId))
        {
            items[item.itemId].AddQuantity(1);
        }
        else
        {
            items[item.itemId] = new InventoryItem(item.itemId, UID);
        }

        SaveInventory();
    }

    public void Clear()
    {
        this.items = new Dictionary<string, InventoryItem>();
    }

    public void RemoveItem(ItemData item)
    {
        if (items.ContainsKey(item.itemId))
        {
            items[item.itemId].RemoveQuantity(1);
            if (items[item.itemId].Quantity <= 0)
            {
                items.Remove(item.itemId);
            }
        }

        SaveInventory();
    }

    public bool HasItem(string itemId) => items.ContainsKey(itemId);

    public List<InventoryItem> GetAllItems()
    {
        return new List<InventoryItem>(items.Values);
    }

    public void SaveInventory()
    {
        string json = JsonUtility.ToJson(new InventoryData(items));
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    public void LoadInventory()
    {
        if (PlayerPrefs.HasKey(SaveKey))
        {
            string json = PlayerPrefs.GetString(SaveKey);
            InventoryData data = JsonUtility.FromJson<InventoryData>(json);
            items = data.ToDictionary();
        }
    }
}
