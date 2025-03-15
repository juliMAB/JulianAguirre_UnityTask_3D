using UnityEngine;

public class DropedItem : MonoBehaviour
{
    InventoryItemModel item = null;

    public void Initialize(InventoryItemModel item)
    {
        this.item = item;
    }

    public void OnPickUp(out InventoryItemModel model)
    {
        model = item;
        Destroy(gameObject);
    }
}
