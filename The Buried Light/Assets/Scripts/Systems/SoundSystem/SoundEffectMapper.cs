using System.Collections.Generic;
using UnityEngine;

public class SoundEffectMapper
{
    private readonly Dictionary<System.Type, string> _commandToSoundMap = new();

    public SoundEffectMapper()
    {
        InitializeMappings();
    }

    /// <summary>
    /// Sets up the mappings between commands and sound IDs.
    /// </summary>
    private void InitializeMappings()
    {
        _commandToSoundMap[typeof(PlayerShootCommand)] = "shoot";
        _commandToSoundMap[typeof(EnemyDamageCommand)] = "enemy_damage";
        _commandToSoundMap[typeof(EnemyKilledCommand)] = "enemy_killed";
    }

    /// <summary>
    /// Retrieves the sound ID for a given command type.
    /// </summary>
    public string GetSoundIdForCommand(ICommand command)
    {
        if (command == null)
        {
            Debug.LogWarning("SoundEffectMapper: Command is null.");
            return null;
        }

        if (_commandToSoundMap.TryGetValue(command.GetType(), out var soundId))
        {
            return soundId;
        }

        Debug.LogWarning($"SoundEffectMapper: No sound mapped for command type {command.GetType().Name}.");
        return null;
    }

    /// <summary>
    /// Adds or updates a mapping between a command and a sound ID.
    /// </summary>
    public void AddOrUpdateMapping<TCommand>(string soundId) where TCommand : ICommand
    {
        _commandToSoundMap[typeof(TCommand)] = soundId;
    }
}
