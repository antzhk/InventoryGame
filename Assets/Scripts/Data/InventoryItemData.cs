namespace Data
{
    [System.Serializable]
    public class InventoryItemData
    {
        public string itemId;
        public int quantity;

        public InventoryItemData(string id, int qty)
        {
            itemId = id;
            quantity = qty;
        }
    }
}