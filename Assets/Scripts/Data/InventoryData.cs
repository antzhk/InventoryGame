using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public class InventoryData
    {
        public List<InventoryItemData> items = new List<InventoryItemData>();

        public InventoryData(Dictionary<string, InventoryItem> inventory)
        {
            foreach (var item in inventory.Values)
            {
                items.Add(new InventoryItemData(item.ItemId, item.Quantity));
            }
        }

        public Dictionary<string, InventoryItem> ToDictionary()
        {
            Dictionary<string, InventoryItem> dict = new Dictionary<string, InventoryItem>();
            
            List<ItemData> itemsList = Resources.LoadAll<ItemData>("").ToList();
            
            foreach (var itemData in items)
            {
                foreach (var item in itemsList)
                {
                    if (item != null && item.itemId.Equals(itemData.itemId))
                    {
                        dict[itemData.itemId] = new InventoryItem(item.itemId, itemData.quantity);
                    }
                }
            }
            return dict;
        }
    }
}