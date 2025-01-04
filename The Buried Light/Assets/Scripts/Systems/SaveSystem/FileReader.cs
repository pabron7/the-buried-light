using UnityEngine;
using System.IO;
using Cysharp.Threading.Tasks;

public class FileReader : IFileReader
{
    private readonly string _basePath;

    public FileReader()
    {
        _basePath = Application.persistentDataPath;
    }

    public async UniTask<string> ReadFromFileAsync(string fileName)
    {
        string path = GetFilePath(fileName);
        using (StreamReader reader = new StreamReader(path))
        {
            return await reader.ReadToEndAsync();
        }
    }

    public async UniTask<bool> FileExistsAsync(string fileName)
    {
        string path = GetFilePath(fileName);
        return await UniTask.FromResult(File.Exists(path));
    }

    private string GetFilePath(string fileName)
    {
        return Path.Combine(_basePath, $"{fileName}.json");
    }
}

