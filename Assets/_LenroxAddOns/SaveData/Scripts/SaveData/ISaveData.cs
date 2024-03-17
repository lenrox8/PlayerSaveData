using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveData
{
    public PlayerSaveData playerSaveData { get; set; }
    public virtual void Clear()
    {

    }
    public virtual void SaveAllData()
    {

    }
    public virtual void SavePlayerData()
    {

    }
}
