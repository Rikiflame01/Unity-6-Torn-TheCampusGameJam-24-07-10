using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class SettingsManager : MonoBehaviour
{
    [Header("Dropdowns")]
    public TMP_Dropdown windowModeDropdown;
    public TMP_Dropdown qualityDropdown;

    private void Start()
    {
        InitializeWindowModeDropdown();
        InitializeQualityDropdown();

        // Load the saved settings
        LoadSettings();
    }

    private void InitializeWindowModeDropdown()
    {
        windowModeDropdown.ClearOptions();
        windowModeDropdown.AddOptions(new List<string> { "Fullscreen", "Windowed", "Borderless" });
        windowModeDropdown.onValueChanged.AddListener(SetWindowMode);
        windowModeDropdown.value = PlayerPrefs.GetInt("WindowMode", 0);
    }

    private void InitializeQualityDropdown()
    {
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(new List<string> { "Low", "Medium", "High", "Ultra" });
        qualityDropdown.onValueChanged.AddListener(SetQuality);
        qualityDropdown.value = PlayerPrefs.GetInt("Quality", 2);
    }

    public void SetWindowMode(int index)
    {
        switch (index)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                break;
        }
        PlayerPrefs.SetInt("WindowMode", index);
        PlayerPrefs.Save();
    }

    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
        PlayerPrefs.SetInt("Quality", index);
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        int windowMode = PlayerPrefs.GetInt("WindowMode", 0);
        SetWindowMode(windowMode);
        windowModeDropdown.value = windowMode;

        int quality = PlayerPrefs.GetInt("Quality", 2);
        SetQuality(quality);
        qualityDropdown.value = quality;
    }
}
