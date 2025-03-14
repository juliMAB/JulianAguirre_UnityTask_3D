using System.Collections.Generic;
using UnityEngine;

namespace scripts.UI
{
    public class UIInventoryManager : MonoBehaviour
    {
        private UIInventoryItem GrabedItem = null;
        private UIInventorySlot OverSlot = null;
        private UIInventorySlot LastOverSlot = null;
        private List<UIInventorySlot> Slots = null;

        [SerializeField] private int slotCuantity = 8;
        [SerializeField] private GameObject SlotPrefab = null;
        [SerializeField] private GameObject InventoryContent = null;

        private void Start()
        {
            Slots = new List<UIInventorySlot>();
            for (int i = 0; i < slotCuantity; i++)
            {
                CreateNewSlot(i);
            }
        }
        private void Update()
        {
            OverSlot = GetHoverSlot();
            if (Input.GetMouseButtonDown(0))
            {
                if (!OverSlot)
                    return;

                if (LastOverSlot != OverSlot) 
                    LastOverSlot = OverSlot;
                
                GrabedItem = OverSlot.item;

                GrabedItem.transform.SetParent(InventoryContent.transform);

                OverSlot.item = null;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (!GrabedItem) return;

                if (!OverSlot)
                {
                    DummyDrop();
                }
                else
                {
                    if (!OverSlot.item)
                    {
                        OverSlot.SetItem(GrabedItem);
                    }
                    else
                    {
                        LastOverSlot.SetItem(GrabedItem);
                        LastOverSlot = null;
                    }
                }
                
                GrabedItem = null;
            }
            else if (Input.GetMouseButton(0))
            {
                if (GrabedItem)
                    GrabedItem.transform.position = Input.mousePosition;
            }
        }
        private UIInventorySlot GetHoverSlot()
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(Slots[i].RectTransform, Input.mousePosition))
                {
                    return Slots[i];
                }
            }
            return null;
        }
        private void CreateNewSlot(int index)
        {
            UIInventorySlot slot = Instantiate(SlotPrefab, InventoryContent.transform).GetComponent<UIInventorySlot>();
            Slots.Add(slot);
            slot.name = "slot :" + index.ToString();
        }
        private void DummyDrop()
        {
            Debug.Log("Drop");
            Destroy(GrabedItem.gameObject);
        }
    }
}