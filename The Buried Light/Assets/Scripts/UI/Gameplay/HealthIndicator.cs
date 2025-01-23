using UnityEngine;
using TMPro;
using UniRx;
using Zenject;

/// <summary>
/// Displays the player's current health in a TextMeshPro UI element.
/// </summary>
public class HealthIndicator : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText; // Reference to the TextMeshPro component for displaying health
    private PlayerHealth playerHealth;

    /// <summary>
    /// Injects the PlayerHealth dependency via Zenject.
    /// </summary>
    /// <param name="playerHealth">The PlayerHealth instance to monitor.</param>
    [Inject]
    public void Construct(PlayerHealth playerHealth)
    {
        this.playerHealth = playerHealth;
    }

    private void Start()
    {
        if (healthText == null)
        {
            Debug.LogError("HealthIndicator: TextMeshPro reference is missing!");
            return;
        }

        if (playerHealth == null)
        {
            Debug.LogError("HealthIndicator: PlayerHealth is not injected!");
            return;
        }

        if (playerHealth.CurrentHealth == null)
        {
            Debug.LogError("HealthIndicator: CurrentHealth is not initialized!");
            return;
        }

        // Subscribe to the reactive property to update the UI whenever health changes
        playerHealth.CurrentHealth
            .Subscribe(health =>
            {
                healthText.text = $"Health: {health}";
                Debug.Log($"HealthIndicator: Updated health to {health}.");
            })
            .AddTo(this); // Automatically disposes of the subscription when the GameObject is destroyed
    }
}
