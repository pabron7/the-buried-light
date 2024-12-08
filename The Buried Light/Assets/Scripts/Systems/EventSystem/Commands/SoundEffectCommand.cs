using UnityEngine;

public class SoundEffectCommand : ICommand
{
    public SoundEffect Effect { get; }

    public SoundEffectCommand(SoundEffect effect)
    {
        Effect = effect;
    }

    public void Execute()
    {
       
    }
}

