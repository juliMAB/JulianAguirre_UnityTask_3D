using System;
using System.Collections.Generic;
using UnityEngine;

public class DropedItemsScene : MonoBehaviour
{
    [SerializeField] List<DropedItem> dropedItems = new List<DropedItem>();

    public void Inizialited(Func<InventoryItemModel, bool> onPickUp)
    {
        for (int i = 0; i < dropedItems.Count; i++)
        {
            dropedItems[i].Initialize(onPickUp);
        }
    }
}
