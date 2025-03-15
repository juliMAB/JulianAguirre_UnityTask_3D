using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace scripts.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIInventoryItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler,IDragHandler , IPointerClickHandler
    {
        #region Exposed_Fields
        [SerializeField] private Image image = null;
        [SerializeField] private TextMeshProUGUI cuantity_text = null;
        #endregion
        #region Private_Fields
        private Action<InventoryItemModel> OnGrab = null;
        private Action<PointerEventData, InventoryItemModel> OnDrop = null;
        private Action<InventoryItemModel, UIInventoryItem> OnTryUse = null;
        private CanvasGroup canvasGroup = null;
        private InventoryItemModel model = null;
        #endregion
        #region PublicProperties
        public Action<PointerEventData, InventoryItemModel> OnDrop1 { get => OnDrop; set => OnDrop = value; }
        public InventoryItemModel Model { get => model;}
        #endregion



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

        public void SetValues(InventoryItemModel model, Action<InventoryItemModel> OnGrab, Action<InventoryItemModel,UIInventoryItem> OnTryUse)
        {
            if (model == null) return;
            if (model.id == -1)
                return;
            this.OnGrab = OnGrab;
            this.OnTryUse = OnTryUse;
            
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

        public void OnPointerClick(PointerEventData eventData)
        {
            OnTryUse?.Invoke(model, this);
        }
    }
}
