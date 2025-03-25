using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class InventoryPanel : MonoBehaviour
    {
        [SerializeField] private List<InventorySlot> slots;
        
        private CanvasGroup canvasGroup;
        private BackPack backPack;
        
        private static InventoryPanel instance;

        private void Awake()
        {
            this.canvasGroup = this.GetComponent<CanvasGroup>();
            this.canvasGroup.alpha = 0f;
            
            this.Hide();

            if (instance == null)
                instance = this;
        }

        private void RefreshPanel()
        {
            foreach (var snapedItem in backPack.GetSnapedItems())
            {
                foreach (var slot in this.slots)
                {
                    if (slot.itemType == snapedItem.GetItemType())
                    {
                        slot.SetItem(snapedItem.GetItem());
                        continue;
                    }
                }
                
            }
        }

        public void Show(BackPack backPack, InventoryManager inventoryManager)
        {
            this.canvasGroup.alpha = 1f;
            
            this.backPack = backPack;

            foreach (var slot in slots)
            {
                slot.SetSlot(backPack);
            }
            
            this.RefreshPanel();
        }

        public void Hide()
        {
            foreach (var slot in slots)
            {
                slot.RemoveItem();
            }
            this.canvasGroup.alpha = 0f;
        }

        public static InventoryPanel Get()
        {
            return instance;
        }
        
        public bool IsActive => this.canvasGroup.alpha != 0;
    }
}