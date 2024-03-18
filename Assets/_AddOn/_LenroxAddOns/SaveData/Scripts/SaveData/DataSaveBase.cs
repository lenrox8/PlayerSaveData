using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DataSaveBase
{
    private static bool _dataSaved = true;
    private string _loadButtonName => GetLoadButtonName();
    private string _saveButtonName => GetSaveButtonName();
    [OnInspectorInit]
    protected virtual void Init()
    {
        LoadAllData();
    }

    [HorizontalGroup("Split", Order = -1)]
    [Button(ButtonSizes.Large,Name = "@this._saveButtonName")]
    public virtual void SaveAllData()
    {
        _dataSaved = true;

    }
    [HorizontalGroup("Split", Order = -1)]
    [Button(ButtonSizes.Large,Name = "@this._loadButtonName")]
    public virtual void LoadAllData()
    {
        _dataSaved = true;
    }
    [Button(ButtonSizes.Large), VerticalGroup("Clear", -1), GUIColor("red"), PropertySpace(0, 20)]
    public virtual void Clear()
    {
       _dataSaved = false;
    }
    private string GetLoadButtonName()
    {
        return _dataSaved ? "Load Data" : "Restore Erased Data";
    }
    private string GetSaveButtonName()
    {
        return _dataSaved ? "Save Data" : "*Save Cleared Data";
    }
}
