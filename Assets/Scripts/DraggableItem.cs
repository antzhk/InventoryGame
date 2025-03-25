using System;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class DraggableItem : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private float snapDuration = 3f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float moveForce = 10f; // Сила перемещения
    [SerializeField] private float maxSpeed = 5f;   // Ограничение скорости
    
    public string UID;
    
    public bool isSnap;
    
    public static UnityEvent<DraggableItem, BackPack> OnItemAdded = new UnityEvent<DraggableItem, BackPack>();
    public static UnityEvent<DraggableItem, BackPack> OnItemRemoved = new UnityEvent<DraggableItem, BackPack>();
    
    private Rigidbody rb;
    private bool isDragging = false;
    private Camera mainCamera;
    private bool isOverBackpack = false;
    private Transform backpackTransform;
    
    private static List<DraggableItem> draggableItems;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = itemData.weight;
        mainCamera = Camera.main;

        if (draggableItems == null)
            draggableItems = new List<DraggableItem>();
        
        draggableItems.Add(this);
    }

    private void OnDestroy()
    {
        draggableItems.Remove(this);
    }

    private void Update()
    {
        if (isDragging)
        {
            MoveItemToMouse();
        }
    }

    private void OnMouseDown()
    {
        if (isSnap)
            return;
        
        isDragging = true;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
    }

    private void OnMouseUp()
    {
        isDragging = false;
        rb.useGravity = true;

        if (isOverBackpack && backpackTransform != null)
        {
            AttachToBackpack();
        }
    }

    public ItemType GetItemType()
    {
        return itemData.itemType;
    }

    public ItemData GetItem()
    {
        return itemData;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Backpack"))
        {
            isOverBackpack = true;
            backpackTransform = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Backpack"))
        {
            isOverBackpack = false;
            backpackTransform = null;
        }
    }

    private void MoveItemToMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        {
            Vector3 direction = (hit.point - transform.position).normalized;
            rb.AddForce(direction * moveForce, ForceMode.Acceleration);

            // Ограничение скорости, чтобы не разгонялся бесконечно
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
        }
    }

    public void AttachToBackpack()
    {
        rb.isKinematic = true;
        var backPack = backpackTransform.GetComponent<BackPack>();
        backPack.AddSnapItem(this.itemData, this);
        
        this.isSnap = true;
        OnItemAdded?.Invoke(this, backPack);
        
        transform.SetParent(backPack.GetPosition(itemData));
        transform.DOLocalMove(Vector3.zero, snapDuration);
        transform.DOLocalRotate(Vector3.zero, snapDuration);
    }

    public void InstantAttach(BackPack backPack) // After load game
    {
        rb.isKinematic = true;
        
        this.isSnap = true;
        
        transform.SetParent(backPack.GetPosition(itemData));
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void RemoveFromBackPack(BackPack backPack)
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            Vector3 cameraForward = new Vector3(-mainCamera.transform.forward.x, 1, -mainCamera.transform.forward.z).normalized;
            Vector3 targetPosition = backPack.transform.position + cameraForward * 2;
      
            this.transform.SetParent(null);
            
            this.transform.DOMove(targetPosition, 1f).OnComplete(() =>
            {
                if (rb != null)
                {
                    rb.isKinematic = false;
                    this.isSnap = false;
                    OnItemRemoved?.Invoke(this, backPack);
                }
            });
        }
    }

    public static void InitDraggableItems()
    {
        foreach (var draggableItem in draggableItems)
        {
            var itemdatas = DraggableItemSaveSystem.LoadItems();

            if (!DraggableItemSaveSystem.HasSave())
                return;

            bool hasItem = false;
            
            foreach (var itemData in itemdatas)
            {
                if (itemData.UID.Equals(draggableItem.UID))
                {
                    draggableItem.transform.position = itemData.position;
                    hasItem = true;
                    break;
                }
            }
            
            if (!hasItem)
                Destroy(draggableItem.gameObject);
        }
    }

    public static void SaveDraggableItems()
    {
        var items = new List<DraggableItem>();
        
        foreach (var draggableItem in draggableItems)
        {
            if (!draggableItem.isSnap)
            {
                items.Add(draggableItem);
            }
        }
        
        DraggableItemSaveSystem.SaveItems(items);
    }
}
