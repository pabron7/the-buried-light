using Cysharp.Threading.Tasks;
public interface ILoadHandler
{
    UniTask<T> LoadAsync<T>(string fileName) where T : class, new();
    UniTask<bool> FileExistsAsync(string fileName);
}
