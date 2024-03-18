using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class FileWriter
{
    private const string _DESKey = "Cl4G21p5";

    public static void SaveDataFile<T>(ref T data, string saveFolder, string path) where T : class
    {
        string filePath = Application.persistentDataPath + saveFolder + path;
        string json = JsonUtility.ToJson(data);
        string encodedJson = Encryptor.EncryptToBase64(json, _DESKey);
        File.WriteAllText(filePath, encodedJson);
    }
    public static void LoadData<T>(ref T data, string saveFolder, string path) where T : class
    {
        string filePath = Application.persistentDataPath + saveFolder + path;
        if (File.Exists(filePath))
        {
            string jsonEncoded = File.ReadAllText(filePath);
            string json = Encryptor.DecryptFromBase64(jsonEncoded, _DESKey);
            data = JsonUtility.FromJson<T>(json);
        }
        else
        {
            data = Activator.CreateInstance(typeof(T)) as T;
            SaveDataFile<T>(ref data, saveFolder, path);
        }
    }
}
