using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip enemyDamageSound;
    [SerializeField] private AudioClip enemyKilledSound;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("SoundManager requires an AudioSource component.");
        }
    }

    public void PlayShootSound()
    {
        PlaySound(shootSound);
    }

    public void PlayEnemyDamageSound()
    {
        PlaySound(enemyDamageSound);
    }

    public void PlayEnemyKilledSound()
    {
        PlaySound(enemyKilledSound);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("SoundManager: Attempted to play a null clip.");
            return;
        }

        _audioSource.PlayOneShot(clip);
    }
}
