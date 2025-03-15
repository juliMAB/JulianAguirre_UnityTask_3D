using System.IO;
using UnityEngine;

public static class DataManager
{
    private const string folderName = "InventoryData";

    public static void SaveData(InventoryModel model)
    {
        string json = JsonUtility.ToJson(model);

        File.WriteAllText(folderName, json);
    }

    public static bool LoadData(out InventoryModel model)
    {
        model = null;

        if (!File.Exists(folderName))
        {
            return false;
        }

        string json = File.ReadAllText(folderName);
        model = JsonUtility.FromJson<InventoryModel>(json);

        return true;
    }
}
