using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public Sprite icon;
    
    public string itemId;
    public string itemName;
    public float weight;
    public ItemType itemType;

    public GameObject gamePrefab;

    protected static List<ItemData> itemsList = new List<ItemData>();

    public static void Load()
    {
        itemsList.Clear();
        itemsList.AddRange(Resources.LoadAll<ItemData>(""));
    }

    public static ItemData GetItemData(string itemId)
    {
        foreach (var itemData in itemsList)
        {
            if (itemData.itemId.Equals(itemId))
                return itemData;
        }

        return null;
    }
}

public enum ItemType
{
    Weapon,
    Food,
    Tool
}
