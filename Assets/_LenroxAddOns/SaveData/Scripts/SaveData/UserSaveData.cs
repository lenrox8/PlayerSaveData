using System.IO;
using UnityEngine;
using Sirenix.OdinInspector;

public class UserSaveData : Singleton<UserSaveData>
{
    [SerializeField]
    private PlayerSaveData _playerData;

    private string _userPreferencesFile = @"/userSaveData.dat";
    private const string _encryptKey = "al8s1c12";

    public PlayerSaveData playerSaveData => _playerData;
    private string _saveFilePath => Application.persistentDataPath + _userPreferencesFile;
    private void Awake()
    {
        _playerData = new PlayerSaveData();
        Init();
    }
    private void Init() // Load data on init
    {
        Load();
    }
    public void Load() //Loading data 
    {
        if (File.Exists(_saveFilePath))
        {
            string json = Encryptor.DecryptFromBase64(File.ReadAllText(_saveFilePath), _encryptKey);
            var data = JsonUtility.FromJson<PlayerSaveData>(json);
            _playerData = data;
        }
        else Debug.Log("Save Not Exist");
    }
    [Button, HorizontalGroup("Buttons",.5f)]
    public void Save() //Saving data 
    {
        string encodedJson = Encryptor.EncryptToBase64(JsonUtility.ToJson(_playerData), _encryptKey);
        File.WriteAllText(_saveFilePath, encodedJson);
    }
    [Button, HorizontalGroup("Buttons", .5f)]
    public void Clear() //Clear save data
    {
        _playerData = new PlayerSaveData();
        Save();
    }
}
