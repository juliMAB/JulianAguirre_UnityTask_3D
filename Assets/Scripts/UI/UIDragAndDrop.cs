using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;
namespace scripts.UI
{
    [Serializable]
    public class UIDragAndDrop
    {

        [SerializeField] private UIInventoryItem GrabedItem = null;
        [SerializeField] private UIInventorySlot fromSlot = null;
        [SerializeField] private GameObject ItemPrefab = null;

        [SerializeField] private Transform Inventory;

        private Action<InventoryItemModel, UIInventorySlot, UIInventorySlot> onDrop;
        public void Initialize(Action<InventoryItemModel, UIInventorySlot, UIInventorySlot> onDrop)
        {
            this.onDrop = onDrop;
        }
        public void Update()
        {
            if (GrabedItem)
            {
                GrabedItem.transform.position = Input.mousePosition;
            }
        }

        public void StartDrag(UIInventorySlot fromSlot)
        {
            UIInventoryItem item = fromSlot.Item;
            item.transform.SetParent(Inventory);
            this.fromSlot = fromSlot;
            GrabedItem = item;
            item.OnDrop1 = ItemWasDrop;
        }
        public void TrySwitchItemsUI(UIInventorySlot fromSlot, UIInventorySlot toSlot, UIInventoryItem item)
        {
            if (!toSlot) return; //no slot.
            if (toSlot.HasItem)
            {
                SwitchItems(fromSlot,toSlot);
            }
            else
            {
                toSlot.SetItem(item);
                fromSlot.RemoveItem();
            }
            item.OnDrop1 = null;
            item = null;
            fromSlot = null;
            GrabedItem = null;
        }
        public void ConsumeItem(UIInventorySlot fromSlot, UIInventoryItem item)
        {
            fromSlot.RemoveItem();
            item.OnDrop1 = null;
            fromSlot = null;
            GrabedItem = null;
        }

        private void ItemWasDrop(PointerEventData eventData, InventoryItemModel model)
        {
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            UIInventorySlot toSlot = null;
            foreach (RaycastResult result in results)
            {
                toSlot = result.gameObject.GetComponent<UIInventorySlot>();
                if (toSlot != null)
                    break;
            }

            if (toSlot)
            {
                if (toSlot.equipmentType == EquipmentType.None)
                {
                    Debug.Log("Drop item at: " + toSlot);
                    onDrop?.Invoke(model, toSlot, fromSlot);
                    TrySwitchItemsUI(fromSlot, toSlot, GrabedItem);
                    return;
                }
                else
                {
                    ItemSO item_so = ItemsController.Instance.GetItem(model.id);
                    if (item_so.EquipType == toSlot.equipmentType)
                    {
                        if (!toSlot.HasItem)
                        {
                            Debug.Log("Drop item at: " + toSlot);
                            onDrop?.Invoke(model, toSlot, fromSlot);
                            TrySwitchItemsUI(fromSlot, toSlot, GrabedItem);
                            return;
                        }
                    }
                }
            }
            Debug.Log("Drop item at: " + fromSlot);
            onDrop?.Invoke(model, fromSlot, fromSlot);
            TrySwitchItemsUI(fromSlot, fromSlot, GrabedItem);
        }
        public void SwitchItems(UIInventorySlot fromSlot,UIInventorySlot toSlot)
        {
            UIInventoryItem temp_A = fromSlot.Item;
            UIInventoryItem temp_B = toSlot.Item;
            fromSlot.RemoveItem();
            toSlot.RemoveItem();
            fromSlot.SetItem(temp_B);
            toSlot.SetItem(temp_A);
        }
    }

}