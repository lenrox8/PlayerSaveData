using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;
using System;

[ShowOdinSerializedPropertiesInInspector, System.Serializable]
public class SaveData: DataSaveBase, ISaveData
{
    [SerializeField, ReadOnly, HideLabel,GUIColor("green"), BoxGroup("Data")]
    private string _saveId = string.Empty;

    [SerializeField, BoxGroup("Data")] 
    private PlayerSaveData _playerSaveData;

    public PlayerSaveData playerSaveData { get { return _playerSaveData; } set { _playerSaveData = value; } }

    private string _playerSaveFile = @"/playerSave.dat";

    private string _saveFolder;
    private SaveController _controller;
    public SaveData(string saveId, string saveFolder, SaveController saveController)
    {
        this._saveFolder = saveFolder;
        this._saveId = saveId;
        _controller = saveController;
    }
    [OnInspectorInit]
    protected override void Init()
    {
        LoadAllData();
    }
    public override void SaveAllData()
    {
        base.SaveAllData(); 
        SavePlayerData();
        SaveActions.SaveChanged?.Invoke();
    }
    public override void LoadAllData()
    {
        base.LoadAllData();
        FileWriter.LoadData<PlayerSaveData>(ref _playerSaveData,_saveFolder, _playerSaveFile);
    }
    public override void Clear()
    {
        base.Clear();
        _playerSaveData = new PlayerSaveData();
    }

    [Button(ButtonSizes.Medium), FoldoutGroup("Save Buttons",Order = -1)]
    public void SetActiveSave()
    {
        _controller.SetActiveSave(_saveId);
    }
    [Button(ButtonSizes.Medium), FoldoutGroup("Save Buttons", Order = -1), GUIColor("red")]
    public void EraseSave()
    {
        _controller.EraseSave(_saveId);
    }
    [Button(ButtonSizes.Medium), FoldoutGroup("Save Buttons", Order = -1)]
    public void SavePlayerData()
    {
        FileWriter.SaveDataFile<PlayerSaveData>(ref _playerSaveData,_saveFolder, _playerSaveFile);
    }
    [Button(ButtonSizes.Medium,Expanded = true,Style = ButtonStyle.Box), FoldoutGroup("Save Buttons", Order = -1)]
    public void ChangeSaveName(string newName)
    {
        _controller.ChangeSaveName(_saveId, newName);
    }
}
