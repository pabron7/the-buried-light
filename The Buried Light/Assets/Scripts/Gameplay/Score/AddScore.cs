using UnityEngine;
using UniRx;

public class AddScore : IAddScore
{
    public void Execute(ReactiveProperty<int> currentScore, int valueToAdd)
    {
        currentScore.Value += valueToAdd;
    }
}
