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

        public Action<UIInventorySlot, InventoryItemModel> OnGrabItem = null;

        public void Initialize(InventoryItemModel item, int id,Action<UIInventorySlot, InventoryItemModel> OnGrabItem, bool isEquipment = false, EquipmentType equipmentType = default)
        {
            this.id = id;
            this.isEquipment = isEquipment;
            this.equipmentType = equipmentType;

            if (item.IsEmpty)
            {
                return;
            }

            this.item.SetValues(item, GrabItem);
            this.OnGrabItem = OnGrabItem;
        }

        public void SetItem(UIInventoryItem item)
        {
            if (this.item)
            {
                Destroy(item.gameObject);
            }
            this.item = item;
            item.transform.position = transform.position;
            item.transform.SetParent(transform);
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
            OnGrabItem?.Invoke(this, model);
        }
    }
}

