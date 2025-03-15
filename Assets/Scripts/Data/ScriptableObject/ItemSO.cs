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
}
