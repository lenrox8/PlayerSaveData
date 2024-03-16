using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveController
{
    [SerializeField, ReadOnly]
    private string _currentSaveDataName;

    [SerializeField]
    private SaveData _currentSaveData;

    public SaveData currentSaveData
    {
        get
        {
            if (_currentSaveData == null) _currentSaveData = GetCurrentSave();
            return _currentSaveData;
        }
        private set { _currentSaveData = value; }
    }

    public Action onUpdateTreeRequired;
    [OnInspectorInit]
    private void Init()
    {
        _currentSaveData = GetCurrentSave();
    }

    [PropertyOrder(-1),Button(ButtonSizes.Large), HorizontalGroup("Split", .5f), GUIColor("green")]
    public void CreateNewSave()
    {
        string directoryPath = SaveDataFilesPaths._savePath;
        var info = new DirectoryInfo(directoryPath);
        var directoryInfo = info.GetDirectories();
        var saveFolderName = $"{SaveDataFilesPaths._saveDirectoryName}_{directoryInfo.Length}";
        var directoryName = $"{directoryPath}/{saveFolderName}";

        if (!Directory.Exists(directoryName)) Directory.CreateDirectory(directoryName);

        var saveData = new SaveData($"{SaveDataFilesPaths._saveFolder}/{saveFolderName}");
        saveData.LoadAllData();

        onUpdateTreeRequired?.Invoke();
    }
    [PropertyOrder(-1), Button(ButtonSizes.Large), HorizontalGroup("Split",.5f), GUIColor("red")]
    public void ClearAllSaves()
    {
        string directoryPath = SaveDataFilesPaths._savePath;
        var info = new DirectoryInfo(directoryPath);
        var directoryInfo = info.GetDirectories();

        foreach (var directory in directoryInfo) directory.Delete(true);

        CreateNewSave();
    }
    public SaveData GetCurrentSave()
    {
        string directoryPath = SaveDataFilesPaths._savePath;

        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

        var info = new DirectoryInfo(directoryPath);
        var directoryInfo = info.GetDirectories();
        directoryInfo = directoryInfo.OrderBy(x => x.LastWriteTime).ToArray();

        if(directoryInfo.Length == 0) return null;

        var saveData = new SaveData($"{SaveDataFilesPaths._saveFolder}/{directoryInfo.Last().Name}");
        saveData.LoadAllData();

        _currentSaveDataName = $"{directoryInfo.Last().Name}";

        return saveData;
    }
}

public static class SaveDataFilesPaths
{
    public static string _saveDirectoryName = "/Save";
    public static string _saveFolder = "/SaveDatas";
    public static string _savePath => Application.persistentDataPath + _saveFolder;
}
