using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.IO;
using Sirenix.Serialization;
using System.Linq;

public class SaveEditorWindow : OdinMenuEditorWindow
{
    private OdinMenuTree tree;
    private SaveController saveController;

#if UNITY_EDITOR
    [MenuItem("Save/Edit Save")]
    private static void OpenWindow()
    {
        GetWindow<SaveEditorWindow>().Show();
    }
    protected override OdinMenuTree BuildMenuTree()
    {
        saveController = new SaveController();
        saveController.onUpdateTreeRequired += NewSaveCreated;

        tree = new OdinMenuTree();
        tree.Selection.SupportsMultiSelect = false;

        tree.Add("Save Controller", saveController);
        BuildSaveTree();

        return tree;
    }
    private void NewSaveCreated()
    {
        ForceMenuTreeRebuild();
    }
    private void BuildSaveTree()
    {
        string directoryPath = SaveDataFilesPaths._savePath;

        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

        var info = new DirectoryInfo(directoryPath);
        var directoryInfo = info.GetDirectories();
        directoryInfo = directoryInfo.OrderBy(x => x.LastWriteTime).ToArray();

        string currentSaveId = saveController.GetGameSave().gameData.currentSaveId;

        for (int i = 0; i < directoryInfo.Length; i++)
        {
            DirectoryInfo directory = directoryInfo[i];
            var saveData = new SaveData(directory.Name,$"{SaveDataFilesPaths._saveFolder}/{directory.Name}", saveController);
            saveData.LoadAllData();
            tree.Add($"Saves/{directory.Name}", saveData, directory.Name == currentSaveId? SdfIconType.ArchiveFill : SdfIconType.Archive);
        }
    }
    protected override void OnDestroy()
    {
        saveController.onUpdateTreeRequired -= NewSaveCreated;
    }
#endif
}