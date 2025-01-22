using UnityEngine;

public abstract class MainMenuTab : MonoBehaviour
{
    public virtual void Show()
    {
        gameObject.SetActive(true);
        Debug.Log($"{name} tab is now active.");
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
        Debug.Log($"{name} tab is now inactive.");
    }
}
