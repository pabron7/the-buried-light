using Cysharp.Threading.Tasks;
using UnityEngine;
using System.IO;

public class FileWriter : IFileWriter
{
    private readonly string _basePath;

    public FileWriter()
    {
        _basePath = Application.persistentDataPath;
    }

    public async UniTask WriteToFileAsync(string fileName, string content)
    {
        string path = GetFilePath(fileName);
        using (StreamWriter writer = new StreamWriter(path))
        {
            await writer.WriteAsync(content);
        }
    }

    private string GetFilePath(string fileName)
    {
        return Path.Combine(_basePath, $"{fileName}.json");
    }
}

