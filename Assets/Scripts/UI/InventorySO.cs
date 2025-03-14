using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InventorySO : ScriptableObject
{
    [SerializeField]
    private List<InventoryItem> inventoryItems;
    private int Size => inventoryItems.Count;
    [SerializeField]
    private InventoryItem GrabedItem;

    public void Initialize()
    {
        inventoryItems = new List<InventoryItem>();
        for (int i = 0; i < Size; i++)
        {
            inventoryItems.Add(InventoryItem.GetEmptyItem());
        }
    }

    public void AddItem(ItemSO item, int quantity)
    {
        for (int i = 0; i < Size; i++)
        {
            if (inventoryItems[i].IsEmpty)
            {
                inventoryItems[i] = new InventoryItem
                {
                    item = item,
                    quantity = quantity
                };
            }
        }
    }
    public void GrabItem(int index)
    {
        GrabedItem = inventoryItems[index];
        inventoryItems[index] = InventoryItem.GetEmptyItem();
    }

    public void PutItemGrabedItem(int index, int last_index)
    {
        if (GrabedItem.IsEmpty) return;

        if (inventoryItems[index].IsEmpty)
        {
            inventoryItems[index] = GrabedItem;
            GrabedItem = InventoryItem.GetEmptyItem();
        }
        else
        {
            InventoryItem temp = inventoryItems[index];
            inventoryItems[index] = GrabedItem;
            inventoryItems[last_index] = temp;
            GrabedItem = InventoryItem.GetEmptyItem();
        }
    }
    public void PutNewItem(int index, InventoryItem item)
    {
        inventoryItems[index] = item;
    }
    public void RemoveItem(int index)
    {
        inventoryItems[index] = InventoryItem.GetEmptyItem();
    }
    public InventoryItem GetGrabed()
    {
        return GrabedItem;
    }
    public List<InventoryItem> GetItems()
    {
        return inventoryItems;
    }
    public int getSize()
    {
        return inventoryItems.Count;
    }
}

[Serializable]
public struct InventoryItem
{
    public int quantity;
    public ItemSO item;

    public bool IsEmpty => item == null;

    public InventoryItem ChangeQuantity(int newQuantity)
    {
        return new InventoryItem
        {
            item = this.item,
            quantity = newQuantity,
        };
    }
    public static InventoryItem GetEmptyItem()
    {
        return new InventoryItem
        {
            item = null,
            quantity = 0
        };

    }
}
