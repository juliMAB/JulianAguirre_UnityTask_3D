using scripts.UI;
using System;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private InventoryManager inventoryManager = null;
    [SerializeField] private CharacterStats characterStats = null;

    private Action<InventoryItemModel> ConsumeItem = null;
    private Action<float> addLife = null;
    private Action<float> addHungry = null;
    void Start()
    {
        ConsumeItem += EvalutateItemUse;
        inventoryManager.Initialize(ConsumeItem);

        characterStats.Initialize(out addLife,out addHungry);
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

    
}
