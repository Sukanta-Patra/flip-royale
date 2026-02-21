using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class ScoreData
{
    public List<int> scores = new List<int>();
}

public static class SaveManager
{
    private static string FilePath =>
        Path.Combine(Application.persistentDataPath, "scores.json");

    private const int MAX_SCORES = 5;

    private static ScoreData cachedData;

    public static ScoreData Load()
    {
        if (cachedData != null)
            return cachedData;

        if (File.Exists(FilePath))
        {
            string json = File.ReadAllText(FilePath);
            cachedData = JsonUtility.FromJson<ScoreData>(json);
        }
        else
        {
            cachedData = new ScoreData();
        }

        return cachedData;
    }

    public static void AddScore(int newScore)
    {
        ScoreData data = Load();

        data.scores.Add(newScore);
        data.scores.Sort((a, b) => b.CompareTo(a));

        if (data.scores.Count > MAX_SCORES)
            data.scores.RemoveRange(MAX_SCORES, data.scores.Count - MAX_SCORES);

        Save();
    }

    private static void Save()
    {
        string json = JsonUtility.ToJson(cachedData, true);
        File.WriteAllText(FilePath, json);
    }

    public static List<int> GetScores()
    {
        return new List<int>(Load().scores);
    }
}
