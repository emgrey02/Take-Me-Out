using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine.InputSystem;
using System.Linq;
using System.Collections.Generic;

public class MenuController : MonoBehaviour
{
    [SerializeField] InputReader inputReader;

    private GameObject Inventory;
    private VisualElement invPanel;
    private VisualElement settingsMenu;
    private VisualElement initialMenu;
    private VisualElement mm;

    // buttons
    public Button playButton;
    public Button settingsButton;
    public Button quitButton;
    public Button saveButton;

    public DropdownField qualityDropdown;
    public SliderInt masterVolSlider;
    public SliderInt musicVolSlider;
    public SliderInt sfxVolSlider;
    public SliderInt lookSenSlider;
    public SliderInt walkSpeedSlider;

    void Awake()
    {
        // get visual elements
        mm = GetComponent<UIDocument>().rootVisualElement;
        settingsMenu = mm.Q<VisualElement>("settingsMenu");
        initialMenu = mm.Q<VisualElement>("initialMenu");

        // get buttons
        playButton = mm.Q<Button>("PlayButton");
        settingsButton = mm.Q<Button>("SettingsButton");
        saveButton = mm.Q<Button>("SaveButton");
        quitButton = mm.Q<Button>("QuitButton");

        // get dropdowns and sliders
        qualityDropdown = mm.Q<DropdownField>("quality");
        masterVolSlider = mm.Q<SliderInt>("mastervol");
        musicVolSlider = mm.Q<SliderInt>("musicvol");
        sfxVolSlider = mm.Q<SliderInt>("sfxvol");
        lookSenSlider = mm.Q<SliderInt>("lookSensitivity");
        walkSpeedSlider = mm.Q<SliderInt>("walkSpeed");
        
        // populate enum dropdown
        qualityDropdown.choices = QualitySettings.names.ToList();
  
    }

    void OnEnable()
    {
        // subscribe to click events
        playButton.clicked += OnPlayButtonClicked;
        settingsButton.clicked += OnSettingsButtonClicked;
        saveButton.clicked += OnSaveButtonClicked;       
        quitButton.clicked += OnQuitButtonClicked;

        // subscribe to main menu toggle event
        inputReader.MainMenuToggleEvent += OnMainMenuToggle;
    }

    void OnDisable()
    {
        // unsubscribe to click events
        playButton.clicked -= OnPlayButtonClicked;
        settingsButton.clicked -= OnSettingsButtonClicked;
        saveButton.clicked -= OnSaveButtonClicked;       
        quitButton.clicked -= OnQuitButtonClicked;

        // unsubscribe to main menu toggle event
        inputReader.MainMenuToggleEvent -= OnMainMenuToggle;
    }

    void Start()
    {

        // get inventory ui
        Inventory = GameObject.FindWithTag("Inventory");
        invPanel = Inventory.GetComponent<UIDocument>().rootVisualElement;

        // hide menu if not scene 0
        if (GameManager.Instance.GetSceneId() != 0)
        {
            mm.AddToClassList("hide");
        } else
        {
            inputReader.DisablePlayerControls();
        }

        // set play button text depending on scene num
        if (GameManager.Instance.GetSceneId() == 0 )
        {
            playButton.text = "New Game";
        }
        else 
        {
            playButton.text = "Continue";
        }

        
        // set current graphics quality level
        qualityDropdown.index = QualitySettings.GetQualityLevel();

        // set current look sensitivity & walk speed
        PlayerSaveData data = GameManager.Instance.GetPlayerData();
        if (data != null)
        {
            lookSenSlider.value = data.lookSensitivity;
            walkSpeedSlider.value = data.moveSpeed; 
        }
        else {
            lookSenSlider.value = 20;
            walkSpeedSlider.value = 4;
        }

    }

    private void OnQuitButtonClicked()
    {
        GameManager.Instance.ClearInventory();

        Application.Quit();
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #endif
    }

    private void OnSettingsButtonClicked()
    {
        Debug.Log("Settings");
        initialMenu.AddToClassList("remove");
        settingsMenu.RemoveFromClassList("remove");
    }

    private void OnSaveButtonClicked()
    {
        Debug.Log("Save Game");
        settingsMenu.AddToClassList("remove");
        initialMenu.RemoveFromClassList("remove");

        // set graphics quality
        GameManager.Instance.SetGraphicsQuality(qualityDropdown.index);

        // set volumes
        // implement here

        // set player data
        GameManager.Instance.SetPlayerData(lookSenSlider.value, walkSpeedSlider.value);
    }

    private void OnPlayButtonClicked()
    {
        if (GameManager.Instance.GetSceneId() == 0)
        {
            GameManager.Instance.MoveToScene(1);
        }
        else
        {
            // hide main menu
            mm.AddToClassList("hide");
        }
    }

    void OnMainMenuToggle(bool MainMenuToggled)
    {
        // if we toggled opened main menu with TAB and inventory isnt open
        if (MainMenuToggled && invPanel.ClassListContains("hide"))
        {
            Debug.Log("main menu toggle triggered");
            mm.ToggleInClassList("hide");

            // get current playercontrols status
            bool status = inputReader.PlayerControlsStatus();
            Debug.Log("Player controls status: ");
            Debug.Log(status);

            // disable/enable player controls if main menu is on screen or not
            if (mm.ClassListContains("hide"))
            {
                // if main menu is hidden, go back to what controls were before toggling main menu
                if (status)
                {
                    // if they were on before, turn them back on, if they were off, leave them off
                    Debug.Log("enabling player controls");
                    inputReader.EnablePlayerControls(); 
                }

            }
            else {
                Debug.Log("disabling player controls");
                inputReader.DisablePlayerControls();
            }
        }
    }
    
}
