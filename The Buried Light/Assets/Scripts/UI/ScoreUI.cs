using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

public class ScoreUI : MonoBehaviour
{
    [Inject] private ScoreManager _scoreManager;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        // Bind the score to the UI
        _scoreManager.CurrentScore
            .Subscribe(score => scoreText.text = $"Score: {score}")
            .AddTo(this);
    }
}
