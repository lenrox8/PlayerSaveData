using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class GameSaveData: DataSaveBase
{
    [SerializeField, BoxGroup("Data")]
    private GameData _gameData;

    public GameData gameData { get { return _gameData; } set { _gameData = value; } }

    private string _gameSaveFile = @"/gameData.dat";
    private string _saveFolder;
    public GameSaveData(string saveFolder)
    {
        this._saveFolder = saveFolder;
    }
    protected override void Init()
    {
        LoadAllData();
    }
    public override void SaveAllData()
    {
        base.SaveAllData();
        SaveGameData();
    }
    public override void LoadAllData()
    {
        base.LoadAllData();
        FileWriter.LoadData<GameData>(ref _gameData, _saveFolder, _gameSaveFile);
    }
    public override void Clear()
    {
        base.Clear();
        _gameData = new GameData();
    }
    public void SaveGameData()
    {
        FileWriter.SaveDataFile<GameData>(ref _gameData, _saveFolder, _gameSaveFile);
    }
}
