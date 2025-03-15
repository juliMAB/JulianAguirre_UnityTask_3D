using System.Collections.Generic;
using UnityEngine;

public class ItemsController : MonoBehaviour
{
    public static ItemsController Instance = null;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    
    [SerializeField] private List<ItemSO> items = null;
    
    public ItemSO GetItem(int id)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if(items[i].ID == id)
            {
                return items[i];
            }
        }
        return null;
    }
    public List<ItemSO> GetItems(List<int> ids)
    {
        List<ItemSO> items = new List<ItemSO>();
        for (int i = 0; i < ids.Count; i++)
        {
            ItemSO item = GetItem(ids[i]);
            if (item != null)
            {
                items.Add(item);
            }
        }
        return items;
    }
}
