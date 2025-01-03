using UniRx;

public class ResetScore : IResetScore
{
    public void Execute(ReactiveProperty<int> currentScore)
    {
        currentScore.Value = 0;
    }
}
