using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace scripts.UI
{
    public class UIInventoryItem : MonoBehaviour
    {
        [SerializeField] private Image image = null;
        [SerializeField] private TextMeshProUGUI cuantity_text = null;
        public void SetValues(InventoryItem data)
        {
            this.image.sprite = data.item.ItemImage;
            if (data.quantity>2)
            {
                this.cuantity_text.text = data.quantity.ToString();
            }
            else
            {
                this.cuantity_text.text = "";
            }
            this.name = data.item.Name; 
        }
    }
}
