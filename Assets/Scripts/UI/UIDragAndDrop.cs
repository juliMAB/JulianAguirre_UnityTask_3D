using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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

        public void StartDrag(UIInventoryItem item, UIInventorySlot fromSlot)
        {
            item.transform.SetParent(Inventory);
            this.fromSlot = fromSlot;
            GrabedItem = item;
            item.OnDrop1 = ItemWasDrop;
        }
        public void EndDrag(UIInventorySlot toSlot)
        {
            if (!toSlot) return; //no slot.
            if (toSlot.HasItem)
            {
                SwitchItems(toSlot);
            }
            else
            {
                toSlot.SetItem(GrabedItem);
            }
            GrabedItem.OnDrop1 = null;
            GrabedItem = null;
            fromSlot = null;
        }

        private void ItemWasDrop(PointerEventData eventData, InventoryItemModel model)
        {
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            UIInventorySlot slot = null;
            foreach (RaycastResult result in results)
            {
                slot = result.gameObject.GetComponent<UIInventorySlot>();
                if (slot != null)
                    break;
            }
            onDrop?.Invoke(model, slot,fromSlot);
            EndDrag(slot);
        }
        public void SwitchItems(UIInventorySlot toSlot)
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