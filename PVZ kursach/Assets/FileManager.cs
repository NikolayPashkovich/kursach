using System.IO; // Для работы с файлами
using UnityEngine;

public static class FileManager
{
    // Путь к файлу сохранения
    private static string FilePath => Path.Combine(Application.persistentDataPath, "savefile.json");

    /// <summary>
    /// Проверяет существование файла и создает новый с базовыми значениями, если его нет.
    /// </summary>
    public static void EnsureFileExists()
    {
        if (!File.Exists(FilePath))
        {
            SaveData defaultData = new SaveData
            {
                levelNumber = 0,
                openPlants = new int[] { 0 }
            };

            SaveData(defaultData);
            Debug.Log("Save file created with default values.");
        }
    }

    /// <summary>
    /// Сохранение всех данных в файл
    /// </summary>
    public static void SaveData(SaveData data)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(FilePath, json);
            Debug.Log("Data saved successfully.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to save data: {ex.Message}");
        }
    }

    /// <summary>
    /// Загрузка всех данных из файла
    /// </summary>
    public static SaveData LoadData()
    {
        try
        {
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);
                return JsonUtility.FromJson<SaveData>(json);
            }
            else
            {
                Debug.LogWarning("Save file not found, creating default data.");
                EnsureFileExists();
                return LoadData();
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to load data: {ex.Message}");
            return new SaveData(); // Возвращаем пустой объект при ошибке
        }
    }

    /// <summary>
    /// Сохранение значения LevelNumber
    /// </summary>
    public static void SaveLevelNumber(int levelNumber)
    {
        SaveData data = LoadData();
        data.levelNumber = levelNumber;
        SaveData(data);
    }

    /// <summary>
    /// Загрузка значения LevelNumber
    /// </summary>
    public static int LoadLevelNumber()
    {
        return LoadData().levelNumber;
    }

    /// <summary>
    /// Сохранение значения OpenPlants
    /// </summary>
    public static void SaveOpenPlants(int[] openPlants)
    {
        SaveData data = LoadData();
        data.openPlants = openPlants;
        SaveData(data);
    }

    /// <summary>
    /// Загрузка значения OpenPlants
    /// </summary>
    public static int[] LoadOpenPlants()
    {
        return LoadData().openPlants;
    }
    public static void DeleteSaveFile()
    {
        try
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
                Debug.Log("Save file deleted successfully.");
            }
            else
            {
                Debug.LogWarning("Save file not found. Nothing to delete.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to delete save file: {ex.Message}");
        }
    }
}

/// <summary>
/// Класс для хранения данных
/// </summary>
[System.Serializable]
public class SaveData
{
    public int levelNumber;
    public int[] openPlants;
}
