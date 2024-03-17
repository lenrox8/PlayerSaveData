using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    [ReadOnly, GUIColor("green")]
    public string currentSaveId = string.Empty;
}
