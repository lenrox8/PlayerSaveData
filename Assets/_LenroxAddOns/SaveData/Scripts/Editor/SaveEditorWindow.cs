using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.IO;
using Sirenix.Serialization;

public class SaveEditorWindow : OdinMenuEditorWindow
{
    [SerializeField] private SaveData saveData;

#if UNITY_EDITOR
    [MenuItem("Save/Edit Save")]
    private static void OpenWindow()
    {
        GetWindow<SaveEditorWindow>().Show();
    }
    protected override OdinMenuTree BuildMenuTree()
    {
        saveData = new SaveData();

        var tree = new OdinMenuTree();
        tree.Selection.SupportsMultiSelect = false;
        
        tree.Add("Player Save Data", saveData);

        return tree;
    }
#endif
}