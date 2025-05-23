using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{

    bool isPaused = false;
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject settingsMenuUI;
    [SerializeField] PlayerInputActions uiControls;
    [SerializeField] Toggle fullscreenToggle, vsyncToggle;
    [SerializeField] TMP_Dropdown resolutionDropdown;
    InputManager inputs; // Reference to the input manager
    Resolution[] osResolutions; // Array of valid screen resolutions reported by the OS
    Dictionary<string, Resolution> resolutions = new Dictionary<string, Resolution>(); // Pairs strings from the dropdown to resolution objects
    int previouslySelectedResolution; // Backup of what resolution the player chose in case the exit without applying

    void Start()
    {
        inputs = InputManager.instance; // Reference the global input manager for controls
        pauseMenuUI.SetActive(false); // Hide pause menu
        settingsMenuUI.SetActive(false);

        /* Determine if full screen toggle and v-sync toggle should be on or off */
        fullscreenToggle.isOn = Screen.fullScreen;
        if (fullscreenToggle.isOn) Screen.fullScreenMode = FullScreenMode.FullScreenWindow; // If fullscreen toggle is on use borderless fullscreen
        vsyncToggle.isOn = QualitySettings.vSyncCount > 0;


        osResolutions = Screen.resolutions; // Get array of usable screen resolutions by the OS
        List<string> dropdownOptions = new List<string>();

        string resolutionOption;
        int initialDropdownValue = -1;
        for (int i = 1; i < osResolutions.Length; i++) // Add the rest of the resolutions that are different (width and height are different)
        {
            resolutionOption = $"{osResolutions[i].width}x{osResolutions[i].height}";
            if (!dropdownOptions.Contains(resolutionOption))
            {
                dropdownOptions.Add(resolutionOption);
                resolutions.Add(resolutionOption, osResolutions[i]);
            }
            if (Screen.width == resolutions[resolutionOption].width && Screen.height == resolutions[resolutionOption].height)
            {
                initialDropdownValue = resolutions.Count;
            }
        }

        resolutionDropdown.AddOptions(dropdownOptions); // Add the resolution strings to the dropdown

        if (initialDropdownValue == -1) // Current resolution not found in reported options, use best resolution available
            resolutionDropdown.value = resolutionDropdown.options.Count;
        else
            resolutionDropdown.value = initialDropdownValue;
    }

    void Update()
    {
        if (inputs.pause.triggered)
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
    public void LoadSettingsMenu()
    {
        settingsMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        previouslySelectedResolution = resolutionDropdown.value; // Store the current resolution in case the user does not apply settings
    }
    public void CloseSettingsMenu()
    {
        settingsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        resolutionDropdown.value = previouslySelectedResolution; // Restore the dropdown to whatever resolution was selected (this is updated in apply settings).
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ApplySettings()
    {
        FullScreenMode screenMode;
        screenMode = fullscreenToggle.isOn ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed; // Select either exclusive fullscreen or windowed
        QualitySettings.vSyncCount = vsyncToggle.isOn ? 1 : 0; // If vsync is toggled on, vsync count should be 1 per frame, otherwise no vsync.

        string resDropdownText = resolutionDropdown.options[resolutionDropdown.value].text; // Get the text from the resolution dropdown
        Resolution newRes = resolutions[resDropdownText];
        previouslySelectedResolution = resolutionDropdown.value;

        Screen.SetResolution(newRes.width, newRes.height, screenMode);
    }
}
