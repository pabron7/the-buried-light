using UnityEngine;
using DG.Tweening;

public abstract class MainMenuTab : MonoBehaviour
{
    [SerializeField] private float animationDuration = 0.3f; 
    [SerializeField] private float scaleFactor = 0.9f;         

    public virtual void Show()
    {
        gameObject.SetActive(true);
        // Start with a slightly smaller scale
        transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        // Animate scaling back to normal size
        transform.DOScale(1f, animationDuration).SetEase(Ease.OutQuad);
        Debug.Log($"{name} tab is now active.");
    }

    public virtual void Hide()
    {
        // Animate scaling down before disabling the tab
        transform.DOScale(scaleFactor, animationDuration).SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
                // Reset scale to default so it shows properly next time
                transform.localScale = Vector3.one;
            });
        Debug.Log($"{name} tab is now inactive.");
    }
}
