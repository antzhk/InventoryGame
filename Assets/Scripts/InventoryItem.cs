using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string UID { get; private set; }   
    public string ItemId { get; private set; }
    public int Quantity { get; private set; }

    public InventoryItem(string itemId, string UID, int quantity = 1)
    {
        this.UID = UID;
        this.ItemId = itemId;
        this.Quantity = quantity;
    }

    public void AddQuantity(int amount) => Quantity += amount;
    public void RemoveQuantity(int amount) => Quantity = Mathf.Max(0, Quantity - amount);
}