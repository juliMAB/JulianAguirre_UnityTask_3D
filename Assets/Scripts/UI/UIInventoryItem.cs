using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace scripts.UI
{
    public class UIInventoryItem : MonoBehaviour
    {
        [SerializeField] private Image image = null;
        [SerializeField] private TextMeshProUGUI cuantity_text = null;
        public void SetValues(Sprite sprite, string cuantity_text,string name)
        {
            this.image.sprite = sprite;
            this.cuantity_text.text = cuantity_text;
            this.name = name; 
        }
    }
}
