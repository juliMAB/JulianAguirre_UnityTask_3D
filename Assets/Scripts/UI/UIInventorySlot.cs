using JetBrains.Annotations;
using UnityEngine;
namespace scripts.UI
{
    public class UIInventorySlot : MonoBehaviour
    {
        private UIInventoryItem item = null;

        private RectTransform rectTransform = null;

        public RectTransform RectTransform => rectTransform;
        public void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }
        public void SetItem(UIInventoryItem item)
        {
            if (this.item)
            {
                Destroy(item.gameObject);
            }
            this.item = item;
            item.transform.position = transform.position;
            item.transform.SetParent(transform);
        }

        public bool HasItem => item;

        public UIInventoryItem RemoveItem()
        {
            UIInventoryItem temp = item;
            item = null;
            return temp;
        }

        public void DestroyItemGO()
        {
            DestroyImmediate(item.gameObject);
        }
        public void SetValues(Sprite sprite, string quantity_text, string name)
        {
            item.SetValues(sprite, quantity_text, name);
        }
    }
}

