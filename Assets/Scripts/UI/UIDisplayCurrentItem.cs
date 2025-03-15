using System;
using TMPro;
using UnityEngine;

[System.Serializable]
public class UIDisplayCurrentItem
{
    [SerializeField] private TextMeshProUGUI itemName_text = null;
    [SerializeField] private TextMeshProUGUI itemDesc_text = null;

    public void Initialize(out Action<string,string> setTexts)
    {
        setTexts = SetTexts;
    }
    private void SetTexts(string name, string desc)
    {
        if (name == null)
        {
            itemName_text.text = "No item hovered";
            itemDesc_text.text = "";
        }
        itemName_text.text = name;
        itemDesc_text.text = desc;
    }
}
