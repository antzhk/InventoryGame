using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using UI;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private Image highlight;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color unSelectedColor;
    
    public ItemType itemType;

    private ItemData storedItem;
    private BackPack backPack;
    private bool isHovered = false;

    public void OnDrop(PointerEventData eventData)
    {
        if (storedItem != null)
        {
            RemoveFromBackpack();
        }
    }

    private void Update()
    {
        if (isHovered)
        {
            if (Input.GetMouseButtonUp(0) && this.storedItem != null)
            {
                RemoveFromBackpack();
                
                InventoryPanel.Get().Hide();
                CameraMove.GetFirst().ChangePoint(PointType.Origin);
            }
        }
    }

    public void SetSlot(BackPack backPack)
    {
        this.backPack = backPack;
    }

    public void SetItem(ItemData itemData)
    {
        if (itemData.itemType != itemType)
            return;

        this.storedItem = itemData;

        this.icon.enabled = true;
        this.highlight.enabled = true;
        
        this.icon.sprite = this.storedItem.icon;
    }

    public void RemoveItem()
    {
        this.icon.enabled = false;
        this.highlight.enabled = false;

        this.storedItem = null;
    }

    private void RemoveFromBackpack()
    {
        var draggableItem = backPack.RemoveSnapItem(this.storedItem);
        
        if (draggableItem != null)
        {
            draggableItem.RemoveFromBackPack(backPack);
        }
        
        RemoveItem();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        highlight.color = selectedColor;
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        highlight.color = unSelectedColor;  
    }
}
