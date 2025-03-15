using System;
using UnityEngine;
using UnityEngine.EventSystems;
namespace scripts.UI
{
    public class UIInventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region Private_Fields
        private UIInventoryItem item = null;

        private Action<UIInventorySlot, InventoryItemModel> onGrabItem = null;

        private Action<InventoryItemModel, UIInventoryItem, UIInventorySlot> onClickThisSlot = null;

        private Action<UIInventorySlot> onHoverSlot = null;

        #endregion
        #region Public_Fields
        [SerializeField] private int id = 0;
        [SerializeField] private bool isEquipment = false;
        [SerializeField] private EquipmentType equipmentType = default;
        #endregion
        #region Public_Properties
        public UIInventoryItem Item { get => item; }
        public bool HasItem => item;
        public int Id { get => id;}
        public bool IsEquipment { get => isEquipment;}
        public EquipmentType EquipmentType { get => equipmentType; }
        #endregion

        #region Unity_Methods
        public void OnPointerEnter(PointerEventData eventData)
        {
            onHoverSlot?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            onHoverSlot?.Invoke(null);
        }
        #endregion

        #region Public_Methods
        public void Initialize(InventoryItemModel item, int id, Action<UIInventorySlot,
            InventoryItemModel> OnGrabItem, Action<InventoryItemModel, UIInventoryItem, UIInventorySlot> onClickThisSlot,
            Action<UIInventorySlot> onHoverSlot,
            bool isEquipment = false, EquipmentType equipmentType = EquipmentType.None)
        {
            this.id = id;
            this.isEquipment = isEquipment;
            this.equipmentType = equipmentType;
            this.onGrabItem = OnGrabItem;
            this.onClickThisSlot = onClickThisSlot;
            this.onHoverSlot = onHoverSlot;
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
                item.transform.SetParent(transform);
                item.transform.position = transform.position;
                //item.GetComponent<RectTransform>().localPosition = Vector3.z;
                return; 
            }
            if (this.item)
            {
                Destroy(item.gameObject);
            }
            this.item = item;
            item.transform.SetParent(transform);
            item.transform.position = transform.position;
            //item.GetComponent<RectTransform>().localPosition = Vector3.zero;
            Debug.Log(this.name + " Set item: " + item);
            this.item.SetValues(item.Model, GrabItem, TryUseItem);
        }

        public void RemoveItem()
        {
            item = null;
        }

        #endregion

        #region Private_Methods
        private void GrabItem(InventoryItemModel model)
        {
            onGrabItem?.Invoke(this, model);
        }
        private void TryUseItem(InventoryItemModel model ,UIInventoryItem uiItem)
        {
            onClickThisSlot?.Invoke(model, uiItem, this);
        }

        #endregion
    }
}

