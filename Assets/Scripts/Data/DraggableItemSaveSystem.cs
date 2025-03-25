using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class DraggableItemSaveSystem
    {
        private const string SaveKey = "SavedItems";

        private static DraggableItemDataList draggableItemDatas;

        public static void SaveItems(List<DraggableItem> items)
        {
            List<DraggableItemData> dataList = new List<DraggableItemData>();

            foreach (var item in items)
            {
                dataList.Add(new DraggableItemData(item.transform.position, item.UID, item.GetItem().itemId));
            }
            
            var dataClass = new DraggableItemDataList(dataList);

            string json = JsonUtility.ToJson(dataClass);
            PlayerPrefs.SetString(SaveKey, json);
            PlayerPrefs.Save();

            Debug.Log($"Сохранено {items.Count} предметов в PlayerPrefs");
        }

        public static List<DraggableItemData> LoadItems()
        {
            if (!PlayerPrefs.HasKey(SaveKey))
            {
                Debug.Log("Сохраненные предметы отсутствуют.");
                return new List<DraggableItemData>();
            }

            string json = PlayerPrefs.GetString(SaveKey);
            draggableItemDatas = JsonUtility.FromJson<DraggableItemDataList>(json);

            return draggableItemDatas.items;
        }

        public static bool HasSave()
        {
            return PlayerPrefs.HasKey(SaveKey);
        }

        public static bool HasItem(string UID)
        {
            foreach (var draggableItemData in draggableItemDatas.items)
            {
                if (draggableItemData.UID.Equals(UID))
                    return true;
            }

            return false;
        }

        public static DraggableItemData GetItem(string UID)
        {
            if (HasItem(UID))
            {
                foreach (var draggableItemData in draggableItemDatas.items)
                {
                    if (draggableItemData.UID.Equals(UID))
                    {
                        return draggableItemData;
                    }
                }
            }
            
            return null;
        }
    }
    
    [System.Serializable]
    public class DraggableItemDataList
    {
        public List<DraggableItemData> items;

        public DraggableItemDataList(List<DraggableItemData> items)
        {
            this.items = items;
        }
    }
}