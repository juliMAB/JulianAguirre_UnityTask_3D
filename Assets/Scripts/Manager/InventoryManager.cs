using System;
using System.Collections.Generic;
using UnityEngine;

namespace scripts.UI
{
    public class InventoryManager : MonoBehaviour
    {
        #region Exposed_Fields
        [SerializeField] private List<UIInventorySlot> inventorySlots = null;
        [SerializeField] private List<UIInventorySlot> equipmentsSlots = null;

        [SerializeField] private GameObject prefabItem = null;

        [SerializeField] private GameObject InGameMenu = null;

        [SerializeField] private UIDragAndDrop uIDragAndDrop = new();

        [SerializeField] private UIDisplayCurrentItem uiDisplayCurrentItem = new();

        #endregion

        #region Private_Fields
        private InventoryModel inventoryModel;

        private Action<InventoryItemModel> onConsume = null;
        private Action<InventoryItemModel> onDropToEnviro = null;

        private Action<string, string> onSendToPanel = null;
        #endregion

        public void SetInventory(bool value)
        {
            InGameMenu.SetActive(value);
        }
        public bool getInventoryState()
        {
            return InGameMenu.activeSelf;
        }


        private void HoveringSlot(UIInventorySlot slot)
        {
            if (slot == null)
            {
                onSendToPanel?.Invoke(null, null);
                return;
            }
            if (!slot.HasItem)
            {
                onSendToPanel?.Invoke(null,null);
                return;
            }
            ItemSO item_so = ItemsController.Instance.GetItem(slot.Item.Model.id);
            onSendToPanel?.Invoke(item_so.name, item_so.Description);
        }


        public void Initialize(Action<InventoryItemModel> ConsumeItem, Action<InventoryItemModel> onDropItem)
        {
            onConsume = ConsumeItem;
            onDropToEnviro = onDropItem;
            uiDisplayCurrentItem.Initialize(out onSendToPanel);
            LoadInventory();


            AppliedLoadChanges();
        }
        public void SaveInventory()
        {
            DataManager.SaveData(inventoryModel);
        }
        public void LoadInventory()
        {
            bool dataSaved = DataManager.LoadData(out inventoryModel);

            if (!dataSaved)
            {
                inventoryModel = new InventoryModel(inventorySlots.Count, equipmentsSlots.Count);
                DataManager.SaveData(inventoryModel);
            }
        }
        public void AppliedLoadChanges()
        {
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                if (inventorySlots[i].HasItem)
                {
                    DestroyImmediate(inventorySlots[i].Item.gameObject);
                }
                if (!inventoryModel.InventoryItems[i].IsEmpty)
                {
                    UIInventoryItem item = Instantiate(prefabItem, inventorySlots[i].transform).GetComponent<UIInventoryItem>();
                    InventoryItemModel inventoryItemModel = inventoryModel.InventoryItems[i];
                    inventorySlots[i].SetItem(item);
                    inventorySlots[i].Initialize(inventoryItemModel, i, OnItemGrabbed, OnItemTryUse, HoveringSlot, false);
                }
                else
                {
                    inventorySlots[i].Initialize(inventoryModel.InventoryItems[i], i, OnItemGrabbed, OnItemTryUse, HoveringSlot, false);
                }
                inventorySlots[i].gameObject.name = "InventorySlot_" + i.ToString();
            }

            for (int i = 0; i < equipmentsSlots.Count; i++)
            {
                if (equipmentsSlots[i].HasItem)
                {
                    DestroyImmediate(equipmentsSlots[i].Item.gameObject);
                }
                if (!inventoryModel.EquipedItems[i].IsEmpty)
                {
                    UIInventoryItem item = Instantiate(prefabItem, equipmentsSlots[i].transform).GetComponent<UIInventoryItem>();
                    equipmentsSlots[i].SetItem(item);
                }
                equipmentsSlots[i].Initialize(inventoryModel.EquipedItems[i], GetEquipmentIndex(i), OnItemGrabbed, OnItemTryUse, HoveringSlot, true, (EquipmentType)i);
            }
            uIDragAndDrop.Initialize(OnItemDropped);
        }

        public void ReloadInventory()
        {
            LoadInventory();
            AppliedLoadChanges();
        }

        private void OnItemGrabbed(UIInventorySlot fromSlot, InventoryItemModel item)
        {
            uIDragAndDrop.StartDrag(fromSlot);
        }
        private void OnItemTryUse(InventoryItemModel model, UIInventoryItem uiItem, UIInventorySlot fromSlot)
        {
            ItemSO item_so = ItemsController.Instance.GetItem(model.id);
            if (item_so.EquipType < EquipmentType.Max)
            {
                UIInventorySlot toSlot = equipmentsSlots[(int)item_so.EquipType];
                if (toSlot.HasItem) return;
                if (toSlot.EquipmentType == item_so.EquipType)
                {
                    OnItemDropped(model, toSlot, fromSlot);
                    uIDragAndDrop.TrySwitchItemsUI(fromSlot, toSlot, uiItem);
                }
            }
            if (item_so.IsConsumable)
            {
                onConsume?.Invoke(model);
                OnItemDropped(model, null, fromSlot);
                uIDragAndDrop.ConsumeItem(fromSlot,uiItem);
                Destroy(uiItem.gameObject);
            }
        }

        private void OnItemDropped(InventoryItemModel itemModel,UIInventorySlot toSlot, UIInventorySlot fromSlot)
        {
            int fromEquipId = fromSlot.Id - inventorySlots.Count;
            if (!toSlot) //out slot.
            {
                if (!fromSlot.IsEquipment)
                {
                    inventoryModel.InventoryItems[fromSlot.Id].id = -1;
                }
                else
                {
                    inventoryModel.EquipedItems[fromEquipId].id = -1;
                }
                onDropToEnviro?.Invoke(itemModel);
            }
            else //to other slot.
            {
                int toEquipId = toSlot.Id - inventorySlots.Count;
                
                if (!fromSlot.IsEquipment && !toSlot.IsEquipment)
                {
                    InventoryItemModel temp_a = inventoryModel.InventoryItems[fromSlot.Id];
                    InventoryItemModel temp_b = inventoryModel.InventoryItems[toSlot.Id];
                    inventoryModel.InventoryItems[fromSlot.Id] = temp_b;
                    inventoryModel.InventoryItems[toSlot.Id] = temp_a;
                }
                else if (fromSlot.IsEquipment && toSlot.IsEquipment)
                {
                    return;
                }
                else if (fromSlot.IsEquipment && !toSlot.IsEquipment)
                {
                    InventoryItemModel temp_a = inventoryModel.EquipedItems[fromEquipId];
                    InventoryItemModel temp_b = inventoryModel.InventoryItems[toSlot.Id];
                    inventoryModel.EquipedItems[fromEquipId] = temp_b;
                    inventoryModel.InventoryItems[toSlot.Id] = temp_a;
                }
                else if (!fromSlot.IsEquipment && toSlot.IsEquipment)
                {
                    InventoryItemModel temp_a = inventoryModel.InventoryItems[fromSlot.Id];
                    InventoryItemModel temp_b = inventoryModel.EquipedItems[toEquipId];
                    inventoryModel.InventoryItems[fromSlot.Id] = temp_b;
                    inventoryModel.EquipedItems[toEquipId] = temp_a;
                }
                
            }
            
        }
        private int GetEquipmentIndex(int i)
        {
            return inventorySlots.Count + i; 
        }
        private void Update()
        {
            uIDragAndDrop.Update();
        }
        public void AddRandomItem()
        {
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                if (!inventorySlots[i].HasItem)
                {
                    UIInventoryItem item = Instantiate(prefabItem, inventorySlots[i].transform).GetComponent<UIInventoryItem>();
                    InventoryItemModel inventoryItemModel = new InventoryItemModel(UnityEngine.Random.Range(0,ItemsController.Instance.GetAllItemsAmount()-1), 1);
                    inventorySlots[i].SetItem(item);
                    inventorySlots[i].Initialize(inventoryItemModel, i, OnItemGrabbed, OnItemTryUse, HoveringSlot, false);
                    inventoryModel.InventoryItems[i] = inventoryItemModel;
                    return;
                }
            }
        }
        public void DeleteLastItem()
        {
            for (int i = inventorySlots.Count-1; i >= 0; i--)
            {
                if (inventorySlots[i].HasItem)
                {
                    inventorySlots[i].SetItem(null);
                    inventoryModel.InventoryItems[i] = new InventoryItemModel();
                    return;
                }
            }
        }
    }
}