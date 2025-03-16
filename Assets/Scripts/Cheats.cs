using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// Cheats class is used to test the inventory system.
/// 
/// For now, this script only creates and destroys an item in the inventory. The item is created with Button 1 and destroyed with Button 2.
/// </summary>
public class Cheats : MonoBehaviour
{
    [SerializeField] private GameObject tutorialPanel = null;
    [SerializeField] private CharacterStats playerStats = null;
    [SerializeField] private GameplayManager gameplayManager = null;
    [SerializeField] private ItemSO itemToGenerateByCheat = null;
    [SerializeField] private TMPro.TMP_Dropdown dropdownItems = null;
    [SerializeField] private GameObject cheatPanel = null;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            tutorialPanel.SetActive(!tutorialPanel.activeSelf);
        }
    }
    public void CloseGame()
    {
        Application.Quit();
    }
    public void ResetGame()
    {
        SceneManager.LoadScene(0);
    }
    public void GenerateSelectedItem()
    {
        gameplayManager.GenerateItemDrop(dropdownItems.value);
    }
    public void ResetStats()
    {
        playerStats.Reset();
    }

}
