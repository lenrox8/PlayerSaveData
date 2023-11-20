using UnityEngine;

[System.Serializable]
public class PlayerSaveData
{
    [SerializeField]
    private PlayerProgressSaveData _playerProgress = new PlayerProgressSaveData();

    public PlayerProgressSaveData playerProgress => _playerProgress;
}
