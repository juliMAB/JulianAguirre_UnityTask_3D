using System;
using TMPro;
using UnityEngine;

public class DropedItem : MonoBehaviour
{
    [SerializeField] private float distanceToShowPickup = 2;
    [SerializeField] private TextMeshProUGUI textPickUp = null;
    private Func<InventoryItemModel,bool> onPickUp= null;
    InventoryItemModel item = null;
    Transform target = null;

    public void Initialize(InventoryItemModel item,Transform target,Func<InventoryItemModel,bool> onPickUp)
    {
        this.item = new();
        this.item.id = item.id;
        this.item.quantity = item.quantity;
        this.target = target;
        textPickUp.text = "";
        this.onPickUp = onPickUp;
    }

    private void Update()
    {
        if (Vector3.Distance(target.position,transform.position)< distanceToShowPickup)
        {
            textPickUp.text = "press F to get me";
            if (Input.GetKeyDown(KeyCode.F))
            {
                if(onPickUp.Invoke(item))
                {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            textPickUp.text = "";
        }
    }
}
