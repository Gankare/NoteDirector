using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.IO;

[Serializable]
class AchievementSaveData
{
    public int AmountOfStrawSucks;
}

public class Achivements : MonoBehaviour
{
    // Singleton instance
    #region Singleton
    public static Achivements Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }
    #endregion

    private enum AchievementTier
    {
        None,
        First,
        Second,
        Third
    }

    private int amountOfStrawSucks = 0;
    private AchievementTier currentTier = AchievementTier.None;

    private void Start()
    {
        Load();
    }

    private void Load()
    {
        try
        {
            string jsonString = LoadFile("AchievementData");
            if (!string.IsNullOrEmpty(jsonString))
            {
                AchievementSaveData achievementData = JsonUtility.FromJson<AchievementSaveData>(jsonString);
                amountOfStrawSucks = achievementData.AmountOfStrawSucks;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load achievement data: {e.Message}");
        }

        if (amountOfStrawSucks >= 200)
            currentTier = AchievementTier.Third;
        else if (amountOfStrawSucks >= 100)
            currentTier = AchievementTier.Second;
        else if (amountOfStrawSucks >= 50)
            currentTier = AchievementTier.First;
    }

    private void Save()
    {
        var achievementData = new AchievementSaveData
        {
            AmountOfStrawSucks = amountOfStrawSucks
        };

        string jsonString = JsonUtility.ToJson(achievementData);
        SaveToFile("AchievementData", jsonString);
    }

    private void SaveToFile(string fileName, string jsonString)
    {
        try
        {
            string path = Path.Combine(Application.persistentDataPath, fileName);
            File.WriteAllText(path, jsonString);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save achievement data: {e.Message}");
        }
    }

    private string LoadFile(string fileName)
    {
        try
        {
            string path = Path.Combine(Application.persistentDataPath, fileName);
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            else
            {
                Debug.LogWarning($"File not found: {path}");
                return string.Empty;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to read file: {e.Message}");
            return string.Empty;
        }
    }

    public void SuckMade()
    {
        amountOfStrawSucks++;
        Save();
        CheckAndDisplayAchievements();
    }

    private void CheckAndDisplayAchievements()
    {
        if (amountOfStrawSucks >= 200 && currentTier < AchievementTier.Third)
        {
            Debug.Log("You're a real sucker now: Suck 200 Times");
            currentTier = AchievementTier.Third;
        }
        else if (amountOfStrawSucks >= 100 && currentTier < AchievementTier.Second)
        {
            Debug.Log("Getting the suck of it: Suck 100 Times");
            currentTier = AchievementTier.Second;
        }
        else if (amountOfStrawSucks >= 50 && currentTier < AchievementTier.First)
        {
            Debug.Log("Slerp: Suck 50 Times");
            currentTier = AchievementTier.First;
        }
    }
}
