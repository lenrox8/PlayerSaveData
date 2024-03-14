using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;
using System;

[ShowOdinSerializedPropertiesInInspector, System.Serializable]
public class SaveData: ISaveData
{
    [SerializeField] private PlayerSaveData _playerSaveData;

    public PlayerSaveData playerSaveData { get { return _playerSaveData; } set { _playerSaveData = value; } }

    private const string DESKey = "Cl4G21p5";
    private string userPreferencesFile = @"/userPreferences.dat";


    [Button(ButtonSizes.Medium), PropertyOrder(-1)]
    public void Clear()
    {
        _playerSaveData = new PlayerSaveData();
    }

    [HorizontalGroup("Split", 0.5f)]
    [Button(ButtonSizes.Large), PropertyOrder(-3)]
    public void SaveAllData()
    {
        SaveDataFile<PlayerSaveData>(ref _playerSaveData, userPreferencesFile);  
    }
    [HorizontalGroup("Split", 0.5f)]
    [Button(ButtonSizes.Large), PropertyOrder(-2)]
    public void LoadAllData()
    {
        LoadData<PlayerSaveData>(ref _playerSaveData, userPreferencesFile);
    }
    public static void SaveDataFile<T>(ref T data, string path) where T : class
    {
        string filePath = Application.persistentDataPath + path;
        string json = JsonUtility.ToJson(data);
        string encodedJson = Encryptor.EncryptToBase64(json, DESKey);
        File.WriteAllText(filePath, encodedJson);
    }
    public static void LoadData<T>(ref T data, string path) where T : class
    {
        string filePath = Application.persistentDataPath + path;
        if (File.Exists(filePath))
        {
            string jsonEncoded = File.ReadAllText(filePath);
            string json = Encryptor.DecryptFromBase64(jsonEncoded, DESKey);
            data = JsonUtility.FromJson<T>(json);
        }
        else
        {
            data = Activator.CreateInstance(typeof(T)) as T;
            SaveDataFile<T>(ref data, path);
        }
    }
    [Button]
    public void SavePlayerData()
    {
        SaveDataFile<PlayerSaveData>(ref _playerSaveData, userPreferencesFile);
    }

}
