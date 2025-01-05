using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
using System;

public class SaveHandler : ISaveHandler
{
    private readonly IFileWriter _fileWriter;

    [Inject]
    public SaveHandler(IFileWriter fileWriter)
    {
        _fileWriter = fileWriter;
    }

    public async UniTask SaveAsync<T>(string fileName, T data) where T : class
    {
        try
        {
            string json = JsonUtility.ToJson(data);
            Debug.Log($"Serialized JSON for {fileName}: {json}");
            string encryptedJson = EncryptionUtility.Encrypt(json);
            await _fileWriter.WriteToFileAsync(fileName, encryptedJson);
            Debug.Log($"Data saved: {fileName}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save {fileName}: {e}");
        }
    }
}
