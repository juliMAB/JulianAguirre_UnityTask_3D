using System;
using System.Collections.Generic;
using UnityEngine;

namespace scripts.UI
{
    public class UIInventoryManager : MonoBehaviour
    {
        private UIInventoryItem GrabedItem = null;
        private UIInventorySlot OverSlot = null;
        private UIInventorySlot LastOverSlot = null;
        private List<UIInventorySlot> Slots = null;
        [SerializeField] private GameObject SlotPrefab = null;
        [SerializeField] private GameObject ItemPrefab = null;
        [SerializeField] private GameObject InventoryContent = null;

        [SerializeField] private InventorySO inventoryData = null;

        private void Start()
        {
            Slots = new List<UIInventorySlot>();
            for (int i = 0; i < inventoryData.getSize(); i++)
            {
                CreateNewSlot(i);
            }
        }
        private void Update()
        {
            OverSlot = GetHoverSlot();
            if (Input.GetMouseButtonDown(0))
            {
                if (!OverSlot)
                    return;

                if (LastOverSlot != OverSlot) 
                    LastOverSlot = OverSlot;
                
                GrabedItem = OverSlot.item;

                GrabedItem.transform.SetParent(InventoryContent.transform);

                OverSlot.item = null;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (!GrabedItem) return;

                if (!OverSlot)
                {
                    DummyDrop();
                }
                else
                {
                    if (!OverSlot.item)
                    {
                        OverSlot.SetItem(GrabedItem);
                    }
                    else
                    {
                        LastOverSlot.SetItem(GrabedItem);
                        LastOverSlot = null;
                    }
                }
                
                GrabedItem = null;
            }
            else if (Input.GetMouseButton(0))
            {
                if (GrabedItem)
                    GrabedItem.transform.position = Input.mousePosition;
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                InventorySendDataToSlots();
            }
        }
        private UIInventorySlot GetHoverSlot()
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(Slots[i].RectTransform, Input.mousePosition))
                {
                    return Slots[i];
                }
            }
            return null;
        }
        private void CreateNewSlot(int index)
        {
            UIInventorySlot slot = Instantiate(SlotPrefab, InventoryContent.transform).GetComponent<UIInventorySlot>();
            Slots.Add(slot);
            slot.name = "slot :" + index.ToString();

            ItemSO currentItem = inventoryData.GetItems()[index].item;
            if (!currentItem) return;

            UIInventoryItem item = Instantiate(ItemPrefab, slot.transform).GetComponent<UIInventoryItem>();

            item.SetValues(currentItem.ItemImage, inventoryData.GetItems()[index].quantity.ToString(), currentItem.Name);

            slot.SetItem(item);
        }
        private void ToggleInventory()
        {
            if(gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
                InventorySendDataToSlots();
            }
        }

        private void SlotsSendDataToInventory()
        {
            
        }
        private void InventorySendDataToSlots()
        {
            int realQuantity = inventoryData.getSize();
            if (realQuantity != Slots.Count) return;
            else
            {
                List<InventoryItem> items = inventoryData.GetItems();
                for (int i = 0; i < realQuantity; i++)
                {
                    ItemSO currentItem = items[i].item;
                    
                    {
                        if (Slots[i].item)
                        {
                            DestroyImmediate(Slots[i].item.gameObject);
                        }

                        if (!currentItem) return;

                        UIInventoryItem item = Instantiate(ItemPrefab, Slots[i].transform).GetComponent<UIInventoryItem>();

                        item.SetValues(currentItem.ItemImage, items[i].quantity.ToString() ,currentItem.Name);

                        Slots[i].SetItem(item);
                    }
                }

            }
        }
        private void DummyDrop()
        {
            Debug.Log("Drop");
            Destroy(GrabedItem.gameObject);
        }
    }
}