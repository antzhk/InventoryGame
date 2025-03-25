using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string ItemId { get; private set; }
    public int Quantity { get; private set; }

    public InventoryItem(string itemId, int quantity = 1)
    {
        ItemId = itemId;
        Quantity = quantity;
    }

    public void AddQuantity(int amount) => Quantity += amount;
    public void RemoveQuantity(int amount) => Quantity = Mathf.Max(0, Quantity - amount);
}