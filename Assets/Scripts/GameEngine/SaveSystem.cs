using UnityEngine;
using System.IO;

public static class SaveSystem
{
    public static bool loadFromSave = false;

    private static string GetPath()
    {
        // persistentDataPath to bezpieczny folder system nie usunie automatycznie
        return Application.persistentDataPath + "/gamesave.json";
    }

    public static void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetPath(), json);
        Debug.Log("zapis w: " + GetPath());
    }

    public static SaveData Load()
    {
        string path = GetPath();
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<SaveData>(json);
        }
        return null;
    }

    public static bool HasSave()
    {
        return File.Exists(GetPath());
    }
}