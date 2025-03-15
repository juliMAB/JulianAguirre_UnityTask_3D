using System;
using UnityEngine;
namespace scripts.UI
{
    public class UIInventorySlot : MonoBehaviour
    {
        private UIInventoryItem item = null;

        public int id = 0;

        public bool isEquipment = false;
        public EquipmentType equipmentType = default;

        public UIInventoryItem Item => item;
        public bool HasItem => item;

        public Action<UIInventorySlot, InventoryItemModel> onGrabItem = null;

        private Action<InventoryItemModel, UIInventoryItem, UIInventorySlot> onClickThisSlot = null;

        public void Initialize(InventoryItemModel item, int id,Action<UIInventorySlot,
            InventoryItemModel> OnGrabItem, Action<InventoryItemModel, UIInventoryItem, UIInventorySlot> onClickThisSlot,
            bool isEquipment = false, EquipmentType equipmentType = EquipmentType.None)
        {
            this.id = id;
            this.isEquipment = isEquipment;
            this.equipmentType = equipmentType;
            this.onGrabItem = OnGrabItem;
            this.onClickThisSlot = onClickThisSlot;
            if (item.IsEmpty)
            {
                return;
            }

            this.item.SetValues(item, GrabItem, TryUseItem);
        }

        public void SetItem(UIInventoryItem item)
        {
            if (item == null)
            {
                if (this.item)
                {
                    Destroy(this.item.gameObject);
                }
                return;
            }
            if (item == this.item) 
            {
                item.transform.position = transform.position;
                item.transform.SetParent(transform);
                return; 
            }
            if (this.item)
            {
                Destroy(item.gameObject);
            }
            this.item = item;
            item.transform.position = transform.position;
            item.transform.SetParent(transform);
            Debug.Log(this.name + " Set item: " + item);
            this.item.SetValues(item.Model, GrabItem, TryUseItem);
        }

        public void RemoveItem()
        {
            item = null;
        }

        public void DestroyItemGO()
        {
            DestroyImmediate(item.gameObject);
        }
        public void GrabItem(InventoryItemModel model)
        {
            onGrabItem?.Invoke(this, model);
        }
        public void TryUseItem(InventoryItemModel model ,UIInventoryItem uiItem)
        {
            onClickThisSlot?.Invoke(model, uiItem, this);
        }
    }
}

