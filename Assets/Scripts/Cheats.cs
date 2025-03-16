using UnityEngine;
/// <summary>
/// Cheats class is used to test the inventory system.
/// 
/// For now, this script only creates and destroys an item in the inventory. The item is created with Button 1 and destroyed with Button 2.
/// </summary>
public class Cheats : MonoBehaviour
{
    [SerializeField] private GameObject tutorialPanel = null;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            tutorialPanel.SetActive(!tutorialPanel.activeSelf);
        }
    }
}
