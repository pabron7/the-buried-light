using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class FPSToggle : MonoBehaviour
{
    private Toggle fpsToggle;

    private void Start()
    {
        fpsToggle = GetComponent<Toggle>();

        if (fpsToggle == null)
        {
            Debug.LogError("FPSToggle: Toggle component is not assigned!");
            return;
        }

        // Initialize the toggle state
        fpsToggle.isOn = Application.targetFrameRate == 60;

        // Add listener for toggle changes
        fpsToggle.onValueChanged.AddListener(OnToggleChanged);
    }

    private void OnToggleChanged(bool isToggled)
    {
        if (isToggled)
        {
            SetFPSLimit(60);
            Debug.Log("FPS limited to 30 for testing.");
        }
        else
        {
            RemoveFPSLimit();
            Debug.Log("FPS limit removed.");
        }
    }

    private void SetFPSLimit(int fps)
    {
        Application.targetFrameRate = fps;
    }

    private void RemoveFPSLimit()
    {
        Application.targetFrameRate = -1; // Removes the FPS limit
    }
}
