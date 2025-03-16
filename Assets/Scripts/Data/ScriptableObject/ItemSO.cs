using UnityEngine;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    [SerializeField]
    public int ID = -1;
    [SerializeField]
    public int MaxStackSize = 1;
    [SerializeField]
    public string Name;
    [SerializeField]
    public string Description;
    [SerializeField]
    public Sprite ItemImage;
    [SerializeField]
    public EquipmentType EquipType;
    [SerializeField]
    public bool IsConsumable;
    [SerializeField]
    public typeConsumable effect = typeConsumable.None;
}
public enum typeConsumable
{
    AddLife,
    RemoveLife,
    AddHungry,
    RemoveHungry,
    None
}
