using UniRx;

public interface IResetScore
{
    void Execute(ReactiveProperty<int> currentScore);
}
