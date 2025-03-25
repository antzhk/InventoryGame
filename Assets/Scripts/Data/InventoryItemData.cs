namespace Data
{
    [System.Serializable]
    public class InventoryItemData
    {
        public string UID;
        public string itemId;
        public int quantity;

        public InventoryItemData(string id, string UID, int qty)
        {
            this.UID = UID;
            this.itemId = id;
            this.quantity = qty;
        }
    }
}