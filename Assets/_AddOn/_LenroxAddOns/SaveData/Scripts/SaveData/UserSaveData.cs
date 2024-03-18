using System.IO;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

public class UserSaveData : Singleton<UserSaveData>, ISaveData
{
    [SerializeField, HideLabel, BoxGroup("Data")]
    private SaveData _saveData;

    private SaveController _controller = new SaveController();
    public PlayerSaveData playerSaveData { get { return _saveData.playerSaveData; } set { _saveData.playerSaveData = value; } }

    private void Awake()
    {
        GetCurrentSave();
    }
    private void OnEnable()
    {
        SaveActions.SaveChanged += GetCurrentSave;
    }
    private void OnDisable()
    {
        SaveActions.SaveChanged -= GetCurrentSave;
    }
    public void ChangeSave(string name)
    {
        _controller.ForceSetActiveSave(name);
        _saveData = _controller.currentSaveData;
        Load();
    }
    public void GetCurrentSave()
    {
        _saveData = _controller.currentSaveData;
        Load();
    }
    public void Load()
    {
        _saveData.LoadAllData();
    }
    [Button]
    public void SaveAllData()
    {
        _saveData.SaveAllData();
    }
    [Button]
    public void Clear()
    {
        _saveData.Clear();
    }
    public void SavePlayerData()
    {
        _saveData.SavePlayerData();
    }
}
