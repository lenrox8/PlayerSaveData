using Sirenix.OdinInspector;
using UnityEngine;

public class Example : MonoBehaviour
{
    private void Start()
    {
        var save = UserSaveData.instance;
    }

    [Button]
    private void ChangeExampleSaveData()
    {
        var playerProgress = UserSaveData.instance.playerSaveData.playerProgress;
        playerProgress.example = !playerProgress.example;

        UserSaveData.instance.SavePlayerData();
    }
}
