using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace scripts.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIInventoryItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler,IDragHandler
    {
        [SerializeField] private Image image = null;
        [SerializeField] private TextMeshProUGUI cuantity_text = null;

        private Action<InventoryItemModel> OnGrab = null;
        private Action<PointerEventData, InventoryItemModel> OnDrop = null;

        private CanvasGroup canvasGroup;

        private InventoryItemModel model = null;

        public Action<PointerEventData, InventoryItemModel> OnDrop1 { get => OnDrop; set => OnDrop = value; }

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();    
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("Dragin: " + name);
            canvasGroup.alpha = .6f;
            canvasGroup.blocksRaycasts = false;
            OnGrab?.Invoke(model);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
            OnDrop?.Invoke(eventData,model);
        }

        public void SetValues(InventoryItemModel model, Action<InventoryItemModel> OnGrab)
        {
            this.OnGrab = OnGrab;
            ItemSO data = ItemsController.Instance.GetItem(model.id);
            this.model = model;
            image.sprite = data.ItemImage;
            if (model.quantity>2)
            {
                cuantity_text.text = model.quantity.ToString();
            }
            else
            {
                cuantity_text.text = "";
            }
            name = data.Name; 
        }

        public void OnDrag(PointerEventData eventData)
        {
            //throw new NotImplementedException();
        }
    }
}
