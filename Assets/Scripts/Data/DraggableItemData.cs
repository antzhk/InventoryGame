using UnityEngine;

namespace Data
{
    
    [System.Serializable]
    public class DraggableItemData
    {
        public Vector3 position;
        public string UID;
        public string itemId;

        public DraggableItemData(Vector3 position, string UID, string itemId)
        {
            this.position = position;
            this.UID = UID;
            this.itemId = itemId;
        }
    }
}