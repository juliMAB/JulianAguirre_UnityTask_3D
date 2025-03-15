using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryModel
{
    [SerializeField]
    private List<InventoryItemModel> inventoryItems;
    [SerializeField]
    private List<InventoryItemModel> equipedItems;

    public InventoryModel(int inventorySlotsAmount, int equipedItemsAmount)
    {
        inventoryItems = new List<InventoryItemModel>();
        equipedItems = new List<InventoryItemModel>();

        for (int i = 0; i < inventorySlotsAmount; i++)
        {
            inventoryItems.Add(new InventoryItemModel());
        }

        for (int i = 0; i < equipedItemsAmount; i++)
        {
            equipedItems.Add(new InventoryItemModel());
        }
    }

    public List<InventoryItemModel> InventoryItems { get => inventoryItems; }
    public List<InventoryItemModel> EquipedItems { get => equipedItems; }    
}

public enum EquipmentType
{
    Head,
    Chest,
    Weapon,
    Feet,
    Max
}