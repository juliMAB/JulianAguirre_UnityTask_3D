using scripts.UI;
using System;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private InventoryManager inventoryManager = null;
    [SerializeField] private CharacterStats characterStats = null;
    [SerializeField] private PlayerController player = null;

    [SerializeField] private GameObject dropItemPrefab = null;

    private bool isInventoryOpen => inventoryManager.getInventoryState();

    private Action<InventoryItemModel> ConsumeItem = null;
    private Action<float> addLife = null;
    private Action<float> addHungry = null;

    private Func<InventoryItemModel,bool> pickUpItem = null;

    void Start()
    {
        ConsumeItem += EvalutateItemUse;

        inventoryManager.Initialize(ConsumeItem, CreateDropAtEnviroment,out pickUpItem);

        characterStats.Initialize(out addLife,out addHungry);


    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            player.enabled = isInventoryOpen;
            inventoryManager.SetInventory(!isInventoryOpen);
            if (isInventoryOpen)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
    private void EvalutateItemUse(InventoryItemModel model)
    {
        ItemSO itemSO = ItemsController.Instance.GetItem(model.id);
        switch (itemSO.effect)
        {
            case typeConsumable.AddLife:
                addLife?.Invoke(.1f);
                break;
            case typeConsumable.RemoveLife:
                addLife?.Invoke(-.1f);
                break;
            case typeConsumable.AddHungry:
                addHungry?.Invoke(.1f);
                break;
            case typeConsumable.RemoveHungry:
                addHungry?.Invoke(-.1f);
                break;
        }
    }

    private void CreateDropAtEnviroment(InventoryItemModel model)
    {
        GameObject dropItem = Instantiate(dropItemPrefab, player.transform.position, Quaternion.identity);
        dropItem.GetComponent<DropedItem>().Initialize(model,player.transform, TryToPickUp);
    }
    private bool TryToPickUp(InventoryItemModel item)
    {
        return pickUpItem(item);
    }
    
}
