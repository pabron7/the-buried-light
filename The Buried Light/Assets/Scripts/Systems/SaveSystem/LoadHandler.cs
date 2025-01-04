using UnityEngine;
using System;
using Zenject;
using Cysharp.Threading.Tasks;

public class LoadHandler : ILoadHandler
{
    private readonly IFileReader _fileReader;

    [Inject]
    public LoadHandler(IFileReader fileReader)
    {
        _fileReader = fileReader;
    }

    public async UniTask<T> LoadAsync<T>(string fileName) where T : class, new()
    {
        try
        {
            if (!await _fileReader.FileExistsAsync(fileName))
            {
                Debug.LogWarning($"File not found: {fileName}. Initializing default data.");
                return new T();
            }

            string encryptedJson = await _fileReader.ReadFromFileAsync(fileName);
            string json = EncryptionUtility.Decrypt(encryptedJson);
            return JsonUtility.FromJson<T>(json);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load {fileName}: {e}");
            return new T();
        }
    }

    public async UniTask<bool> FileExistsAsync(string fileName)
    {
        return await _fileReader.FileExistsAsync(fileName);
    }
}
