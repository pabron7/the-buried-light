using Cysharp.Threading.Tasks;

public interface ISaveHandler
{
    UniTask SaveAsync<T>(string fileName, T data) where T : class;
}