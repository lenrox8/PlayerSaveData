using System.IO;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

public class UserSaveData : Singleton<UserSaveData>, ISaveData
{
    [SerializeField] private SaveController _saveController;

    public PlayerSaveData playerSaveData { get { return _saveController.currentSaveData.playerSaveData; } set { _saveController.currentSaveData.playerSaveData = value; } }

    private void Awake()
    {
        Load();
    }
    public void Load()
    {
        _saveController.currentSaveData.LoadAllData();
    }
    [Button]
    public void SaveAllData()
    {
        _saveController.currentSaveData.SaveAllData();
    }
    [Button]
    public void Clear()
    {
        _saveController.currentSaveData.Clear();
    }
    public void SavePlayerData()
    {
        _saveController.currentSaveData.SavePlayerData();
    }
}
