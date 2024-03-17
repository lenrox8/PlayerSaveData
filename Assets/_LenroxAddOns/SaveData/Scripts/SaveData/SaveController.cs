using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SaveController
{
    [SerializeField, BoxGroup("Current Save"), HideLabel]
    private SaveData _currentSaveData;

    [SerializeField, BoxGroup("Game Data")]
    private bool _showGameData = false;

    [SerializeField, BoxGroup("Game Data"), HideLabel, ShowIf(nameof(_showGameData))]
    private GameSaveData _gameSaveData;
    public GameSaveData gameSaveData
    {
        get
        {
            if (_gameSaveData == null) _gameSaveData = GetGameSave();
            return _gameSaveData;
        }
        private set { _gameSaveData = value; }
    }
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
    public void Init()
    {
        _gameSaveData = GetGameSave();
        _currentSaveData = GetCurrentSave();

        SaveActions.SaveChanged += Update;
        SaveActions.CurrentSaveChanged += Update;
    }
    [OnInspectorDispose]
    private void OnDispose()
    {
        SaveActions.SaveChanged -= Update;
        SaveActions.CurrentSaveChanged -= Update;
    }
    private void Update()
    {
        _currentSaveData = GetCurrentSave();
    }
    [PropertyOrder(-1),Button(ButtonSizes.Large), HorizontalGroup("Split", .5f), GUIColor("green"),PropertySpace(0,20)]
    public void CreateNewSave()
    {
        string directoryPath = SaveDataFilesPaths._savePath;
        var info = new DirectoryInfo(directoryPath);
        var directoryInfo = info.GetDirectories();
        var saveFolderName = $"{SaveDataFilesPaths._saveDirectoryName}_{directoryInfo.Length}";
        var directoryName = $"{directoryPath}/{saveFolderName}";

        if (!Directory.Exists(directoryName)) Directory.CreateDirectory(directoryName);

        var saveData = new SaveData(saveFolderName,$"{SaveDataFilesPaths._saveFolder}/{saveFolderName}",this);
        saveData.LoadAllData();

        onUpdateTreeRequired?.Invoke();
    }
    [PropertyOrder(-1), Button(ButtonSizes.Large), HorizontalGroup("Split",.5f), GUIColor("red"), PropertySpace(0, 20)]
    public void ClearAllSaves()
    {
        string directoryPath = SaveDataFilesPaths._savePath;
        var info = new DirectoryInfo(directoryPath);
        var directoryInfo = info.GetDirectories();

        foreach (var directory in directoryInfo) directory.Delete(true);

        gameSaveData.gameData.currentSaveId = string.Empty;
        gameSaveData.SaveGameData();

        CreateNewSave();
    }
    public SaveData GetCurrentSave()
    {
        string directoryPath = SaveDataFilesPaths._savePath;

        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

        var info = new DirectoryInfo(directoryPath);
        var directoryInfo = info.GetDirectories();
        directoryInfo = directoryInfo.OrderBy(x => x.LastWriteTimeUtc).ToArray();

        if(directoryInfo.Length == 0) return null;

        var saveDataID = gameSaveData.gameData.currentSaveId == "" ? directoryInfo.Last().Name : gameSaveData.gameData.currentSaveId;
        var saveData = new SaveData(saveDataID,$"{SaveDataFilesPaths._saveFolder}/{saveDataID}", this);
        saveData.LoadAllData();

        gameSaveData.gameData.currentSaveId = saveDataID;
        gameSaveData.SaveGameData();

        onUpdateTreeRequired?.Invoke();

        return saveData;
    }
    public GameSaveData GetGameSave()
    {
        string directoryPath = SaveDataFilesPaths._gameSavePath;

        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

        var gameSaveData = new GameSaveData($"{SaveDataFilesPaths._gameSaveFolder}");
        gameSaveData.LoadAllData();

        return gameSaveData;
    }
    public void SetActiveSave(string saveId)
    {
        ForceSetActiveSave(saveId);
        if (UserSaveData.instance != null) UserSaveData.instance.ChangeSave(saveId);
    }
    public void ForceSetActiveSave(string saveId)
    {
        gameSaveData.gameData.currentSaveId = saveId;
        gameSaveData.SaveAllData();

        _currentSaveData = GetCurrentSave();

        onUpdateTreeRequired?.Invoke();
    }
    public void ChangeSaveName(string saveId,string name)
    {
        string directoryPath = SaveDataFilesPaths._savePath;

        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

        var info = new DirectoryInfo(directoryPath);
        var directoryInfo = info.GetDirectories();
        var saveFolder = directoryInfo.ToList().Find(x => x.Name == saveId);
       
        var dirInfo = new DirectoryInfo($"{directoryPath}/{saveFolder.Name}");
        dirInfo.MoveTo($"{directoryPath}/{name}");

        if(gameSaveData.gameData.currentSaveId == saveId)
        {
            gameSaveData.gameData.currentSaveId = name;
            gameSaveData.SaveAllData();
        }

        onUpdateTreeRequired?.Invoke();
    }
    public void EraseSave(string saveId)
    {
        string directoryPath = SaveDataFilesPaths._savePath;

        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

        var info = new DirectoryInfo(directoryPath);
        var directoryInfo = info.GetDirectories();
        var saveFolder = directoryInfo.ToList().Find(x => x.Name == saveId);

        saveFolder.Delete(true);

        if (gameSaveData.gameData.currentSaveId == saveId)
        {
            gameSaveData.gameData.currentSaveId = "";
            gameSaveData.SaveAllData();
        }

        onUpdateTreeRequired?.Invoke();
    }
}
public static class SaveDataFilesPaths
{
    public static string _saveDirectoryName = "/Save";
    public static string _saveFolder = "/SaveDatas";
    public static string _gameSaveFolder = "/GameDatas";
    public static string _savePath => Application.persistentDataPath + _saveFolder;
    public static string _gameSavePath => Application.persistentDataPath + _gameSaveFolder;
}
