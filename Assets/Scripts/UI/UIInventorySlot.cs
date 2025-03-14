using UnityEngine;
namespace scripts.UI
{
    public class UIInventorySlot : MonoBehaviour
    {
        public UIInventoryItem item = null;

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
    }
}

