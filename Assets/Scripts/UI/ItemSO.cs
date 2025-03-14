using UnityEngine;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public int ID => GetInstanceID();
    [SerializeField]
    public int MaxStackSize = 1;
    [SerializeField]
    public string Name;
    [SerializeField]
    public string Description;
    [SerializeField]
    public Sprite ItemImage;
}
