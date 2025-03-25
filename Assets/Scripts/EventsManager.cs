using System;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    private void Awake()
    {
        ItemData.Load();

        DraggableItem.OnItemAdded.AddListener(OnItemAdded);
        DraggableItem.OnItemRemoved.AddListener(OnItemRemoved);
    }

    private void OnDestroy()
    {
        DraggableItem.OnItemAdded.RemoveListener(OnItemAdded);
        DraggableItem.OnItemRemoved.RemoveListener(OnItemRemoved);
    }

    private void Start()
    {
        DraggableItem.InitDraggableItems();
        BackPack.InitBackpacks();
    }

    private void OnItemAdded(DraggableItem item, BackPack backPack)
    {
        DraggableItem.SaveDraggableItems();
        SendItemStatusToServer(item.UID, "Added");
    }

    private void OnItemRemoved(DraggableItem item, BackPack backPack)
    {
        DraggableItem.SaveDraggableItems();
        SendItemStatusToServer(item.UID, "Removed");
    }

    private void SendItemStatusToServer(string itemId, string action)
    {
        string jsonData = $"{{\"item_id\": \"{itemId}\", \"action\": \"{action}\"}}";
    
        StartCoroutine(ServerRequest.SendPostRequest(jsonData));
    }
}
