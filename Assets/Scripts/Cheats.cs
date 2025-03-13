using UnityEngine;
/// <summary>
/// Cheats class is used to test the inventory system.
/// 
/// For now, this script only creates and destroys an item in the inventory. The item is created with Button 1 and destroyed with Button 2.
/// </summary>
public class Cheats : MonoBehaviour
{
    [SerializeField] private GameObject UI_InventoryContent = null;
    [SerializeField] private GameObject UI_InventoryItemPrefab = null;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Instantiate(UI_InventoryItemPrefab, UI_InventoryContent.transform);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            int childs = UI_InventoryContent.transform.childCount;
            if (childs > 1)
            {
                Destroy(UI_InventoryContent.transform.GetChild(childs - 1).gameObject);
            }
        }
    }
}
