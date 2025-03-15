using System.Collections.Generic;
using UnityEngine;

namespace scripts.UI
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private List<UIInventorySlot> inventorySlots = null;
        [SerializeField] private List<UIInventorySlot> equipmentsSlots = null;

        [SerializeField] private GameObject prefabItem = null;

        private InventoryModel model;

        [SerializeField] private UIDragAndDrop uIDragAndDrop = new();

        public void Initialize()
        {
            bool dataSaved = DataManager.LoadData(out model);

            if (!dataSaved)
            {
                model = new InventoryModel(inventorySlots.Count, equipmentsSlots.Count);
                DataManager.SaveData(model);
            }

            for (int i = 0; i < inventorySlots.Count; i++)
            {
                if (!model.InventoryItems[i].IsEmpty)
                {
                    UIInventoryItem item = Instantiate(prefabItem, inventorySlots[i].transform).GetComponent<UIInventoryItem>();
                    InventoryItemModel inventoryItemModel = new InventoryItemModel(1, 1);
                    inventorySlots[i].SetItem(item);
                    inventorySlots[i].Initialize(inventoryItemModel, i, OnItemGrabbed, false);
                    model.InventoryItems[i] = inventoryItemModel;
                }
                else
                {
                    inventorySlots[i].Initialize(model.InventoryItems[i], i, OnItemGrabbed, false);
                }
            }

            for (int i = 0; i < equipmentsSlots.Count; i++)
            {
                equipmentsSlots[i].Initialize(model.EquipedItems[i], GetEquipmentIndex(i), OnItemGrabbed, true, (EquipmentType)i);
            }
            uIDragAndDrop.Initialize(OnItemDropped);
        }

        private void OnItemGrabbed(UIInventorySlot fromSlot, InventoryItemModel item)
        {
            uIDragAndDrop.StartDrag(fromSlot.Item, fromSlot);
        }

        private void OnItemDropped(InventoryItemModel itemModel,UIInventorySlot toSlot, UIInventorySlot fromSlot)
        {
            if (!toSlot) //out slot.
            {
                if (!fromSlot.isEquipment)
                {
                    model.InventoryItems[fromSlot.id].id = -1;
                }
                else
                {
                    model.InventoryItems[fromSlot.id - inventorySlots.Count].id = -1;
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
                        InventoryItemModel temp_a = model.InventoryItems[fromSlot.id];
                        InventoryItemModel temp_b = model.InventoryItems[toSlot.id];
                        model.InventoryItems[fromSlot.id] = temp_b;
                        model.InventoryItems[toSlot.id] = temp_a;
                    }
                    else if(fromSlot.isEquipment && toSlot.isEquipment)
                    {
                        return;
                    }
                    else if (fromSlot.isEquipment && !toSlot.isEquipment)
                    {
                        InventoryItemModel temp_a = model.EquipedItems[fromSlot.id];
                        InventoryItemModel temp_b = model.InventoryItems[toSlot.id];
                        model.EquipedItems[fromSlot.id] = temp_b;
                        model.InventoryItems[toSlot.id] = temp_a;
                    }
                    else if (!fromSlot.isEquipment && toSlot.isEquipment)
                    {
                        InventoryItemModel temp_a = model.InventoryItems[fromSlot.id];
                        InventoryItemModel temp_b = model.EquipedItems[toSlot.id];
                        model.InventoryItems[fromSlot.id] = temp_b;
                        model.EquipedItems[toSlot.id] = temp_a;
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
                        inventorySlots[i].Initialize(inventoryItemModel, i,OnItemGrabbed,false);
                        model.InventoryItems[i] = inventoryItemModel;
                        return;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                DataManager.SaveData(model);
            }
        }
    }
}