using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;

[RequireComponent(typeof(InventoryManager))]
public class BackPack : MonoBehaviour
{
    [SerializeField] private List<ItemPosition> itemPositions;
    
    private Dictionary<ItemType, Transform> positionMap = new Dictionary<ItemType, Transform>();
    private Dictionary<ItemType, DraggableItem> snapedItems = new Dictionary<ItemType, DraggableItem>();

    private InventoryManager inventoryManager;
    
    private bool isCursorOver = false;

    private static List<BackPack> backPacksList = new List<BackPack>();
    private void Awake()
    {
        foreach (var itemPosition in itemPositions)
        {
            this.positionMap[itemPosition.itemType] = itemPosition.position;
        }

        this.inventoryManager = this.GetComponent<InventoryManager>();
        
        backPacksList.Add(this);
        inventoryManager.LoadInventory();
    }

    private void LateUpdate()
    {
        if (isCursorOver && Input.GetMouseButtonDown(0)) 
        {
            InventoryPanel.Get()?.Show(this, this.inventoryManager);
            CameraMove.GetFirst().ChangePoint(PointType.BackPack);
        }
        
        else if (InventoryPanel.Get().IsActive && Input.GetMouseButtonUp(0))
        {
            InventoryPanel.Get().Hide();
            CameraMove.GetFirst().ChangePoint(PointType.Origin);
        }
    }
    
    private void OnMouseEnter()
    {
        isCursorOver = true;
    }

    private void OnMouseExit()
    {
        isCursorOver = false;
    }

    public List<DraggableItem> GetSnapedItems()
    {
        return snapedItems.Values.ToList();
    }
    
    public void AddSnapItem(ItemData itemData, DraggableItem item)
    {
        this.snapedItems[itemData.itemType] = item;
        this.inventoryManager.AddItem(itemData, item.UID);
    }

    public DraggableItem RemoveSnapItem(ItemData itemData)
    {
        if (this.snapedItems.ContainsKey(itemData.itemType))
        {
            var item = this.snapedItems[itemData.itemType];
            
            this.snapedItems.Remove(itemData.itemType);
            
            this.inventoryManager.RemoveItem(itemData);

            return item;
        }

        return null;
    }

    public Transform GetPosition(ItemData itemData)
    {
        return this.positionMap[itemData.itemType];
    }

    public void InitBackPackItems()
    {
        var items = inventoryManager.GetAllItems();

        foreach (var inventoryItem in items)
        {
            var prefab = ItemData.GetItemData(inventoryItem.ItemId).gamePrefab;

            var draggable = Instantiate(prefab).GetComponent<DraggableItem>();
            
            draggable.InstantAttach(this);

            draggable.UID = inventoryItem.UID;

            this.snapedItems.Add(draggable.GetItemType(), draggable);
        }
    }

    public static void InitBackpacks()
    {
        foreach (var backPack in backPacksList)
        {
            backPack.InitBackPackItems();
        }
    }

    public void Clear()
    {
        inventoryManager.Clear();
    }
    
    public static void ClearAll()
    {
        foreach (var backPack in backPacksList)
        {
            backPack.Clear();
        }
    }
}

[System.Serializable]
public class ItemPosition
{
    public Transform position;
    public ItemType itemType;
}
