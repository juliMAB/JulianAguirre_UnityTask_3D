using System;

[Serializable]
public class InventoryItemModel
{
    public int id;
    public int quantity;

    public InventoryItemModel()
    {
        id = -1;
        quantity = 0;
    }

    public InventoryItemModel(int id, int quantity)
    {
        this.id = id;
        this.quantity = quantity;
    }

    public bool IsEmpty => id == -1;

    public void SetQuantity(int newQuantity)
    {
        quantity = newQuantity;
    }
}