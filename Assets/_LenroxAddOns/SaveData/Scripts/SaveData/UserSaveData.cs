using System.IO;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

public class UserSaveData : Singleton<UserSaveData>, ISaveData
{
    [SerializeField] private SaveData _saveData = new SaveData();

    public PlayerSaveData playerSaveData { get { return _saveData.playerSaveData; } set { _saveData.playerSaveData = value; } }

    private void Awake()
    {
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
