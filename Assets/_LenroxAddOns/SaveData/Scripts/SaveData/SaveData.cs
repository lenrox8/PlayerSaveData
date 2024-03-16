using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;
using System;

[ShowOdinSerializedPropertiesInInspector, System.Serializable]
public class SaveData: ISaveData
{
    [SerializeField] private PlayerSaveData _playerSaveData;

    public PlayerSaveData playerSaveData { get { return _playerSaveData; } set { _playerSaveData = value; } }

    private const string _DESKey = "Cl4G21p5";
    private string _playerSaveFile = @"/playerSave.dat";

    private string _saveFolder;
    public SaveData(string saveFolder)
    {
        this._saveFolder = saveFolder;
    }
    [OnInspectorInit]
    private void Init()
    {
        LoadAllData();
    }

    [Button(ButtonSizes.Medium), PropertyOrder(-1)]
    public void Clear()
    {
        _playerSaveData = new PlayerSaveData();
    }

    [HorizontalGroup("Split", 0.5f)]
    [Button(ButtonSizes.Large), PropertyOrder(-3)]
    public void SaveAllData()
    {
        SaveDataFile<PlayerSaveData>(ref _playerSaveData,_saveFolder, _playerSaveFile);  
    }
    [HorizontalGroup("Split", 0.5f)]
    [Button(ButtonSizes.Large), PropertyOrder(-2)]
    public void LoadAllData()
    {
        LoadData<PlayerSaveData>(ref _playerSaveData,_saveFolder, _playerSaveFile);
    }
    public static void SaveDataFile<T>(ref T data,string saveFolder, string path) where T : class
    {
        string filePath = Application.persistentDataPath + saveFolder + path;
        string json = JsonUtility.ToJson(data);
        string encodedJson = Encryptor.EncryptToBase64(json, _DESKey);
        File.WriteAllText(filePath, encodedJson);
    }
    public static void LoadData<T>(ref T data, string saveFolder, string path) where T : class
    {
        string filePath = Application.persistentDataPath + saveFolder + path;
        if (File.Exists(filePath))
        {
            string jsonEncoded = File.ReadAllText(filePath);
            string json = Encryptor.DecryptFromBase64(jsonEncoded, _DESKey);
            data = JsonUtility.FromJson<T>(json);
        }
        else
        {
            data = Activator.CreateInstance(typeof(T)) as T;
            SaveDataFile<T>(ref data, saveFolder, path);
        }
    }
    [Button]
    public void SavePlayerData()
    {
        SaveDataFile<PlayerSaveData>(ref _playerSaveData,_saveFolder, _playerSaveFile);
    }

}
