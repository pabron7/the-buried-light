using UniRx;

public interface IAddScore
{
    void Execute(ReactiveProperty<int> currentScore, int valueToAdd);
}
