using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;

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

        [SerializeField] private ItemSO[] randomItemsToSpawn;

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
            EvaluateAddItem();
            EvaluateElimiteItem();
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

            InventoryItem currentItem = inventoryData.GetItems()[index];
            if (!currentItem.item) return;

            UIInventoryItem item = Instantiate(ItemPrefab, slot.transform).GetComponent<UIInventoryItem>();

            item.SetValues(currentItem);

            slot.SetItem(item);
        }
        private void GrabItem(int index)
        {
            inventoryData.GrabItem(index);
        }
        public void PutGrabedAtNewSlot(int currentSlot, int lastSlot)
        {
            inventoryData.PutItemGrabedItem(currentSlot, lastSlot);
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
                InventoryItem currentItem = items[i];

                {
                    if (currentItem.item == null)
                    {
                        if (Slots[i].HasItem)
                            Slots[i].DestroyItemGO();
                        continue;
                    }
                    else
                    {
                        if (Slots[i].HasItem)
                        {
                            Slots[i].SetValues(currentItem);
                        }
                        else
                        {
                            UIInventoryItem item = Instantiate(ItemPrefab, Slots[i].transform).GetComponent<UIInventoryItem>();

                            item.SetValues(currentItem);

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

        private void EvaluateAddItem()
        {
            if (!OverSlot) return;
            if (OverSlot.HasItem) return;
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                AddItem();
            }
        }
        private void AddItem()
        {
            ItemSO randItem = randomItemsToSpawn[Random.Range(0, randomItemsToSpawn.Length - 1)];
            InventoryItem inventoryItem = new InventoryItem { item = randItem, quantity = 1 };
            inventoryData.PutNewItem(Slots.FindIndex(n => n == OverSlot), inventoryItem);
            UIInventoryItem item = Instantiate(ItemPrefab, OverSlot.transform).GetComponent<UIInventoryItem>();
            item.SetValues(inventoryItem);
            OverSlot.SetItem(item);
        }
        private void EvaluateElimiteItem()
        {
            if (!OverSlot) return;
            if (!OverSlot.HasItem) return;
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                RemoveItem();
            }
        }
        private void RemoveItem()
        {
            InventoryItem inventoryItem = InventoryItem.GetEmptyItem();
            inventoryData.PutNewItem(Slots.FindIndex(n => n == OverSlot), inventoryItem);
            Destroy(OverSlot.RemoveItem().gameObject);
        }
    }
}