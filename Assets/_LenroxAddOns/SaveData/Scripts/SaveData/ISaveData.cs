using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveData
{
    public PlayerSaveData playerSaveData { get; set; }
    public void Clear();
    public void SaveAllData();
    public void SavePlayerData();

}
