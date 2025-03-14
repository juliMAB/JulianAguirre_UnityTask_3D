using System.Collections.Generic;
using UnityEngine;

namespace scripts.UI
{
    public class UIInventoryManager : MonoBehaviour
    {
        [SerializeField] private UIInventoryItem GrabedItem = null;
        [SerializeField] private UIInventorySlot OverSlot = null;
        [SerializeField] private UIInventorySlot LastOverSlot = null;
        [SerializeField] private List<UIInventorySlot> Slots = null;
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
            DragAndDropUpdate();
        }
        private void DragAndDropUpdate()
        {
            OverSlot = GetHoverSlot();
            if (Input.GetMouseButtonDown(0))
            {
                if (OverSlot)
                    GetNewItemFromSlot();
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
                    if (!OverSlot.HasItem)
                    {
                        PutItemInClearSlot();
                    }
                    else
                    {
                        SwapItems();
                    }
                }

                GrabedItem = null;
            }
            else if (Input.GetMouseButton(0))
            {
                if (GrabedItem)
                    GrabedItem.transform.position = Input.mousePosition;
            }
        }
        private void SwapItems()
        {
            LastOverSlot.SetItem(OverSlot.RemoveItem());
            PutItemInClearSlot();
            LastOverSlot = null;
        }
        private void PutItemInClearSlot()
        {
            OverSlot.SetItem(GrabedItem);
            PutGrabedAtNewSlot(Slots.FindIndex(n => n == OverSlot), Slots.FindIndex(n => n == LastOverSlot));
        }
        private void GetNewItemFromSlot()
        {
            if (!OverSlot.HasItem)
                return;
            if (LastOverSlot != OverSlot)
                LastOverSlot = OverSlot;

            GrabedItem = OverSlot.RemoveItem();
            GrabedItem.transform.SetParent(InventoryContent.transform);
            GrabItem(Slots.FindIndex(n => n == OverSlot));
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
        private void GrabItem(int index)
        {
            inventoryData.GrabItem(index);
        }
        public void PutGrabedAtNewSlot(int currentSlot, int lastSlot)
        {
            inventoryData.PutItem(currentSlot, lastSlot);
        }
        [ContextMenu("ForceUpdateInventory")]
        private void InventorySendDataToSlots()
        {
            int realQuantity = inventoryData.getSize();
            int Slots_Count = Slots.Count;
            if (realQuantity != Slots_Count)
            {
                if (realQuantity> Slots_Count)
                {
                    for (int i = 0; i < realQuantity- Slots_Count; i++)
                    {
                        CreateNewSlot(Slots_Count + i);
                    }
                }
                else
                {
                    for (int i = 0; i < Slots_Count - realQuantity; i++)
                    {
                        int index = Slots_Count - i - 1;
                        DestroyImmediate(Slots[index].gameObject);
                        Slots.RemoveAt(index);
                    }
                }
            }
            List<InventoryItem> items = inventoryData.GetItems();
            for (int i = 0; i < realQuantity; i++)
            {
                ItemSO currentItem = items[i].item;

                {
                    if (currentItem == null)
                    {
                        if (Slots[i].HasItem)
                            Slots[i].DestroyItemGO();
                        continue;
                    }
                    else
                    {
                        if (Slots[i].HasItem)
                        {
                            Slots[i].SetValues(currentItem.ItemImage, items[i].quantity.ToString(), currentItem.Name);
                        }
                        else
                        {
                            UIInventoryItem item = Instantiate(ItemPrefab, Slots[i].transform).GetComponent<UIInventoryItem>();

                            item.SetValues(currentItem.ItemImage, items[i].quantity.ToString(), currentItem.Name);

                            Slots[i].SetItem(item);
                        }
                    }
                }
            }
        }
        private void DummyDrop()
        {
            Debug.Log("Drop");
            inventoryData.RemoveItem(Slots.FindIndex(n => n == LastOverSlot));
            Destroy(GrabedItem.gameObject);
        }
    }
}