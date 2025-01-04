using Cysharp.Threading.Tasks;
public interface IFileReader
{
    UniTask<string> ReadFromFileAsync(string fileName);
    UniTask<bool> FileExistsAsync(string fileName);
}

