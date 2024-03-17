using System.IO;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

public class UserSaveData : Singleton<UserSaveData>, ISaveData
{
    [SerializeField]
    private SaveData _saveData;

    private SaveController _controller = new SaveController();
    public PlayerSaveData playerSaveData { get { return _saveData.playerSaveData; } set { _saveData.playerSaveData = value; } }

    private void Awake()
    {
        _saveData = _controller.currentSaveData;
        Load();
    }
    private void OnEnable()
    {
        SaveActions.SaveChanged += Load;
    }
    private void OnDisable()
    {
        SaveActions.SaveChanged -= Load;
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
