using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;
using UniRx;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text movementSpeedText;
    [SerializeField] private TMP_Text damageText;

    private PlayerStatsController _playerStats;

    [Inject]
    public void Construct(PlayerStatsController playerStats)
    {
        _playerStats = playerStats;
    }

    private void Start()
    {
        // Subscribe to changes in player stats
        _playerStats.CurrentHealth.Subscribe(newHealth => UpdateHealthText(newHealth)).AddTo(this);
        _playerStats.MovementSpeed.Subscribe(newSpeed => UpdateSpeedText(newSpeed)).AddTo(this);
        _playerStats.Damage.Subscribe(newDamage => UpdateDamageText(newDamage)).AddTo(this);
    }

    private void OnEnable()
    {
        // Initialize UI with current values
        UpdateHealthText(_playerStats.CurrentHealth.Value);
        UpdateSpeedText(_playerStats.MovementSpeed.Value);
        UpdateDamageText(_playerStats.Damage.Value);
    }

    private void UpdateHealthText(int health)
    {
        healthText.text = $"Health: {health} / {_playerStats.MaxHealth.Value}";
    }

    private void UpdateSpeedText(float speed)
    {
        movementSpeedText.text = $"Speed: {speed:F1}";
    }

    private void UpdateDamageText(int damage)
    {
        damageText.text = $"Damage: {damage}";
    }
}
