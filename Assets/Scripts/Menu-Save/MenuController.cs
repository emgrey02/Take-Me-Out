using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine.InputSystem;
using System.Linq;

public class MenuController : MonoBehaviour
{
    [SerializeField] InputReader inputReader;

    private GameObject Inventory;
    private VisualElement invPanel;
    private VisualElement settingsMenu;
    private VisualElement initialMenu;
    private VisualElement mm;
    private VisualElement startMenu;

    public Image baseImg;
    public Label menuText;

    // buttons
    public Button continueButton;
    public Button settingsButton;
    public Button quitButton;
    public Button saveButton;
    public Button backButton;
    public Button startButton;
    public Button optionsButton;
    public Button exitButton;

    public DropdownField qualityDropdown;
    public SliderInt masterVolSlider;
    public SliderInt musicVolSlider;
    public SliderInt sfxVolSlider;
    public SliderInt lookSenSlider;
    public SliderInt walkSpeedSlider;

    public bool playerControlsEnabled;

    void Awake()
    {
        // get visual elements
        mm = GetComponent<UIDocument>().rootVisualElement;
        settingsMenu = mm.Q<VisualElement>("settingsMenu");
        initialMenu = mm.Q<VisualElement>("initialMenu");
        startMenu = mm.Q<VisualElement>("startMenu");

        // base img
        baseImg = mm.Q<Image>("baseImg");

        // base text
        menuText = mm.Q<Label>("menuText");

        // get buttons
        continueButton = mm.Q<Button>("PlayButton");
        settingsButton = mm.Q<Button>("SettingsButton");
        saveButton = mm.Q<Button>("SaveButton");
        quitButton = mm.Q<Button>("QuitButton");
        backButton = mm.Q<Button>("BackButton");
        startButton = mm.Q<Button>("startBtn");
        optionsButton = mm.Q<Button>("optionsBtn");
        exitButton = mm.Q<Button>("exitBtn");

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
        continueButton.clicked += OnPlayButtonClicked;
        settingsButton.clicked += OnSettingsButtonClicked;
        saveButton.clicked += OnSaveButtonClicked;       
        quitButton.clicked += OnQuitButtonClicked;
        backButton.clicked += OnBackButtonClicked;
        startButton.clicked += OnPlayButtonClicked;
        optionsButton.clicked += OnSettingsButtonClicked;
        exitButton.clicked += OnQuitButtonClicked;

        // subscribe to main menu toggle event
        inputReader.MainMenuToggleEvent += OnMainMenuToggle;
    }

    void OnDisable()
    {
        // unsubscribe to click events
        continueButton.clicked -= OnPlayButtonClicked;
        settingsButton.clicked -= OnSettingsButtonClicked;
        saveButton.clicked -= OnSaveButtonClicked;       
        quitButton.clicked -= OnQuitButtonClicked;
        backButton.clicked -= OnBackButtonClicked;
        startButton.clicked += OnPlayButtonClicked;
        optionsButton.clicked += OnSettingsButtonClicked;
        exitButton.clicked += OnQuitButtonClicked;

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
            settingsMenu.AddToClassList("remove");
            initialMenu.RemoveFromClassList("remove");
            startMenu.AddToClassList("remove");
        } else
        {
            inputReader.DisablePlayerControls();
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

    private void OnBackButtonClicked()
    { 
        if (GameManager.Instance.GetSceneId() == 0)
        {
            startMenu.RemoveFromClassList("remove");
            initialMenu.AddToClassList("remove");
            settingsMenu.AddToClassList("remove");
        } else
        {
            settingsMenu.AddToClassList("remove");
            initialMenu.RemoveFromClassList("remove");
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
        startMenu.AddToClassList("remove");
        settingsMenu.RemoveFromClassList("remove");
    }

    private void OnSaveButtonClicked()
    {
        Debug.Log("Save Game");

        if (GameManager.Instance.GetSceneId() == 0)
        {
            startMenu.RemoveFromClassList("remove");
            initialMenu.AddToClassList("remove");
            settingsMenu.AddToClassList("remove");
        } else
        {
            settingsMenu.AddToClassList("remove");
            initialMenu.RemoveFromClassList("remove");
        }

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
            inputReader.EnablePlayerControls();
            GameManager.Instance.MoveToScene(1);
        }
        else
        {
            // hide main menu
            mm.AddToClassList("hide");

            if (playerControlsEnabled)
            {
                Debug.Log("enabling player controls because they were enabled before opening the main menu");
                inputReader.EnablePlayerControls();
            } else
            {
                Debug.Log("not enabling player controls because they were disabled before opening the menu");
            }
        }
    }

    void OnMainMenuToggle(bool MainMenuToggled)
    {
        // if we toggled opened main menu with TAB and inventory isnt open
        if (MainMenuToggled && invPanel.ClassListContains("hide"))
        {
            Debug.Log("main menu toggle triggered");

            // if main menu is closed
            if (mm.ClassListContains("hide"))
            {
                // get current playercontrols status
                playerControlsEnabled = inputReader.PlayerControlsStatus();
                Debug.Log("Player controls status: ");
                Debug.Log(playerControlsEnabled);

                // get which base scene we are on if we are in a base scene
                int sceneNum = GameManager.Instance.GetSceneId();

                switch (sceneNum)
                {
                    case 2:
                        baseImg.image = Resources.Load<Texture2D>("TMO_pauselocation_1st");
                        menuText.text = "Heyyy, we're at the first base! This is The Burren, where you had your first date with Alison!";
                        break;
                    case 3:
                        baseImg.image = Resources.Load<Texture2D>("TMO_pauselocation_2nd");
                        menuText.text = "This is the second base scene! The Somerville Theatre, where you had your first date with Alsion :)";
                        break;
                    case 4:
                        baseImg.image = Resources.Load<Texture2D>("TMO_pauselocation_3rdb");
                        menuText.text = "This is the third base scene! Echo Lake, where you proposed to Alison!";
                        break;
                    default:
                        baseImg.image = null;
                        menuText.text = null;
                        break;
                }

                // open main menu
                mm.RemoveFromClassList("hide");
                Debug.Log("opening main menu and disabling player controls");
                inputReader.DisablePlayerControls();
            }
            else
            {
                // close main menu
                mm.AddToClassList("hide");
                if (playerControlsEnabled)
                {
                    // if player controls were enabled before opening main menu, re-enable them
                    // if they were disabled before opening main menu, leave them disabled
                    // this way we can return to the correct state after closing main menu
                    Debug.Log("enabling player controls because they were enabled before opening the main menu");
                    inputReader.EnablePlayerControls();
                } else
                {
                    Debug.Log("not enabling player controls because they were disabled before opening the menu");
                }
            }

        }
    }
    
}
