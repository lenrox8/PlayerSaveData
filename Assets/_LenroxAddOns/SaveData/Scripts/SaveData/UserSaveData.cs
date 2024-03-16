using System.IO;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

public class UserSaveData : Singleton<UserSaveData>, ISaveData
{
    [SerializeField]
    private SaveData _currentSaveData;

    public SaveData currentSaveData
    {
        get 
        {
            if (_currentSaveData == null) _currentSaveData = _saveController.currentSaveData;
            return _currentSaveData; 
        }
        set { _currentSaveData = value; }
    }

    private SaveController _saveController = new SaveController();

    public PlayerSaveData playerSaveData { get { return currentSaveData.playerSaveData; } set { currentSaveData.playerSaveData = value; } }

    private void Awake()
    {
        Load();
    }
    public void Load()
    {
        currentSaveData.LoadAllData();
    }
    [Button]
    public void SaveAllData()
    {
        currentSaveData.SaveAllData();
    }
    [Button]
    public void Clear()
    {
        currentSaveData.Clear();
    }
    public void SavePlayerData()
    {
        currentSaveData.SavePlayerData();
    }
}
