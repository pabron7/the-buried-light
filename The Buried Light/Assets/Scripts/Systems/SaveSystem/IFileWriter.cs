using Cysharp.Threading.Tasks;

public interface IFileWriter
{
    UniTask WriteToFileAsync(string fileName, string content);
}