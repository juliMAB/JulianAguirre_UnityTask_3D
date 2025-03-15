using System.Collections.Generic;
using UnityEngine;

namespace scripts.UI
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private List<UIInventorySlot> inventorySlots = null;
        [SerializeField] private List<UIInventorySlot> equipmentsSlots = null;

        [SerializeField] private GameObject prefabItem = null;

        private InventoryModel inventoryModel;

        [SerializeField] private UIDragAndDrop uIDragAndDrop = new();

        public void Initialize()
        {
            bool dataSaved = DataManager.LoadData(out inventoryModel);

            if (!dataSaved)
            {
                inventoryModel = new InventoryModel(inventorySlots.Count, equipmentsSlots.Count);
                DataManager.SaveData(inventoryModel);
            }

            for (int i = 0; i < inventorySlots.Count; i++)
            {
                if (!inventoryModel.InventoryItems[i].IsEmpty)
                {
                    UIInventoryItem item = Instantiate(prefabItem, inventorySlots[i].transform).GetComponent<UIInventoryItem>();
                    InventoryItemModel inventoryItemModel = new InventoryItemModel(1, 1);
                    inventorySlots[i].SetItem(item);
                    inventorySlots[i].Initialize(inventoryItemModel, i, OnItemGrabbed, OnItemTryUse, false);
                    inventoryModel.InventoryItems[i] = inventoryItemModel;
                }
                else
                {
                    inventorySlots[i].Initialize(inventoryModel.InventoryItems[i], i, OnItemGrabbed, OnItemTryUse, false);
                }
            }

            for (int i = 0; i < equipmentsSlots.Count; i++)
            {
                equipmentsSlots[i].Initialize(inventoryModel.EquipedItems[i], GetEquipmentIndex(i), OnItemGrabbed, OnItemTryUse, true, (EquipmentType)i);
            }
            uIDragAndDrop.Initialize(OnItemDropped);
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
                if (toSlot.equipmentType == item_so.EquipType)
                {
                    uIDragAndDrop.TrySwitchItemsUI(fromSlot, toSlot, uiItem);
                }
            }
        }

        private void OnItemDropped(InventoryItemModel itemModel,UIInventorySlot toSlot, UIInventorySlot fromSlot)
        {
            int fromEquipId = fromSlot.id - inventorySlots.Count;
            int toEquipId = toSlot.id - inventorySlots.Count;
            if (!toSlot) //out slot.
            {
                if (!fromSlot.isEquipment)
                {
                    inventoryModel.InventoryItems[fromSlot.id].id = -1;
                }
                else
                {
                    inventoryModel.EquipedItems[fromEquipId].id = -1;
                }
            }
            else //to other slot.
            {
                if (toSlot.HasItem) //ocuped.
                {
                    return; //nothing change.
                }
                else
                {
                    if (!fromSlot.isEquipment && !toSlot.isEquipment)
                    {
                        InventoryItemModel temp_a = inventoryModel.InventoryItems[fromSlot.id];
                        InventoryItemModel temp_b = inventoryModel.InventoryItems[toSlot.id];
                        inventoryModel.InventoryItems[fromSlot.id] = temp_b;
                        inventoryModel.InventoryItems[toSlot.id] = temp_a;
                    }
                    else if(fromSlot.isEquipment && toSlot.isEquipment)
                    {
                        return;
                    }
                    else if (fromSlot.isEquipment && !toSlot.isEquipment)
                    {
                        InventoryItemModel temp_a = inventoryModel.EquipedItems[fromEquipId];
                        InventoryItemModel temp_b = inventoryModel.InventoryItems[toSlot.id];
                        inventoryModel.EquipedItems[fromEquipId] = temp_b;
                        inventoryModel.InventoryItems[toSlot.id] = temp_a;
                    }
                    else if (!fromSlot.isEquipment && toSlot.isEquipment)
                    {
                        InventoryItemModel temp_a = inventoryModel.InventoryItems[fromSlot.id];
                        InventoryItemModel temp_b = inventoryModel.EquipedItems[toEquipId];
                        inventoryModel.InventoryItems[fromSlot.id] = temp_b;
                        inventoryModel.EquipedItems[toEquipId] = temp_a;
                    }

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

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                for (int i = 0; i < inventorySlots.Count; i++)
                {
                    if(!inventorySlots[i].HasItem)
                    {
                        UIInventoryItem item = Instantiate(prefabItem, inventorySlots[i].transform).GetComponent<UIInventoryItem>();
                        InventoryItemModel inventoryItemModel = new InventoryItemModel(1,1);
                        inventorySlots[i].SetItem(item);
                        inventorySlots[i].Initialize(inventoryItemModel, i,OnItemGrabbed,OnItemTryUse,false);
                        inventoryModel.InventoryItems[i] = inventoryItemModel;
                        return;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                DataManager.SaveData(inventoryModel);
            }
        }
    }
}