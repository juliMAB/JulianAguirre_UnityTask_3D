using scripts.UI;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private InventoryManager inventoryManager = null;
    void Start()
    {
        inventoryManager.Initialize();
    }

    
}
