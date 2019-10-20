using System;
using System.IO;
using System.Text;
using UnityEngine;

public class DataHandler : MonoBehaviour
{
    private const string ResultsFile = "res.dat";
    private const string StarsFile = "stars.dat";

    private string _resultsFilePath;
    private string _starsFilePath;

    private void Awake()
    {
        _resultsFilePath = Path.Combine(Application.persistentDataPath, ResultsFile);
        _starsFilePath = Path.Combine(Application.persistentDataPath, StarsFile);

        if (!File.Exists(_resultsFilePath))
        {
            CreateNewResultsFile();
        }

        if (!File.Exists(_starsFilePath))
        {
            CreateNewStarsFile();
        }
    }

    private void CreateNewResultsFile()

    {
        SaveFile(new ResultData(), _resultsFilePath);
    }

    private void CreateNewStarsFile()

    {
        SaveFile(new StarsData(), _starsFilePath);
    }

    public void SaveData(int incorrectSwipes, int stars)
    {
        var resultData = LoadResultsData();
        resultData.incorrectSwipes += incorrectSwipes;

        SaveFile(resultData, _resultsFilePath);

        var starsData = LoadStarsData();
        starsData.starsCollected += stars;
        SaveFile(starsData, _starsFilePath);
    }

    private void SaveFile(object data, string path)
    {
        SaveFile(path, data);
    }

    private ResultData LoadResultsData()
    {
        return LoadData<ResultData>(_resultsFilePath);
    }

    public StarsData LoadStarsData()
    {
        return LoadData<StarsData>(_starsFilePath);
    }

    private static T LoadData<T>(string filePath)
    {
        return JsonUtility.FromJson<T>(
            Encoding.UTF8.GetString(Convert.FromBase64String(File.ReadAllText(filePath))));
    }

    private static void SaveFile(string filePath, object data)
    {
        File.WriteAllText(filePath,
            Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonUtility.ToJson(data))));
    }
}