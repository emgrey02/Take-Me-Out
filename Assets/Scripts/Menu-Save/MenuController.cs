using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine.InputSystem;
using System.Linq;
using Mono.Cecil.Cil;
using FMODUnity;
using FMOD.Studio;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;

public class MenuController : MonoBehaviour
{
    [SerializeField] InputReader inputReader;

    // FMOD Events
    [SerializeField] EventReference startEvent;
    [SerializeField] EventReference continueEvent;
    [SerializeField] EventReference hoverEvent;
    [SerializeField] EventReference quitEvent;
    [SerializeField] EventReference optionsEvent;
    [SerializeField] EventReference backEvent;
    [SerializeField] EventReference saveEvent;
    [SerializeField] EventReference dropdownEvent;
    [SerializeField] EventReference dropdownSelectEvent;
    [SerializeField] EventReference pauseEvent;
    public string pauseSnapshot;

    private GameObject Inventory;
    private VisualElement invPanel;
    private VisualElement settingsMenu;
    private VisualElement initialMenu;
    private VisualElement mm;
    private VisualElement startMenu;

    // FMOD VCAs
    private VCA vcaMasterController;
    private VCA vcaMusicController;
    private VCA vcaSFXController;
    private float lastVol;

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

        // FMOD
        // get VCAs
        vcaMasterController = RuntimeManager.GetVCA("vca:/Master");
        vcaMusicController = RuntimeManager.GetVCA("vca:/Music");
        vcaSFXController = RuntimeManager.GetVCA("vca:/SFX");


    }

    void Update()
    {
        // set volumes on slider change, not on save, so that the user can hear the changes in real time
        vcaMasterController.setVolume(masterVolSlider.value * 0.01f);
        vcaMusicController.setVolume(musicVolSlider.value * 0.01f);
        vcaSFXController.setVolume(sfxVolSlider.value * 0.01f);
    }

    void OnEnable()
    {
        // subscribe to click events
        //continueButton.clicked += OnPlayButtonClicked;
        continueButton.RegisterCallback<ClickEvent, int>(OnPlayButtonClicked, 0);
        settingsButton.clicked += OnSettingsButtonClicked;
        saveButton.clicked += OnSaveButtonClicked;       
        quitButton.clicked += OnQuitButtonClicked;
        backButton.clicked += OnBackButtonClicked;
        //startButton.clicked += OnPlayButtonClicked;
        startButton.RegisterCallback<ClickEvent, int>(OnPlayButtonClicked, 1);
        optionsButton.clicked += OnSettingsButtonClicked;
        exitButton.clicked += OnQuitButtonClicked;

        // dropdown specific handlers
        qualityDropdown.RegisterValueChangedCallback(evt => RuntimeManager.PlayOneShot(dropdownSelectEvent));
        qualityDropdown.RegisterCallback<MouseDownEvent>(OnDropdownDrop);

        // hover event handler
        mm.RegisterCallback<MouseOverEvent>(OnHover, TrickleDown.TrickleDown);

        // subscribe to main menu toggle event
        inputReader.MainMenuToggleEvent += OnMainMenuToggle;
    }

    void OnDisable()
    {
        // unsubscribe to click events
        //continueButton.clicked -= OnPlayButtonClicked;
        continueButton.UnregisterCallback<ClickEvent, int>(OnPlayButtonClicked, 0);
        settingsButton.clicked -= OnSettingsButtonClicked;
        saveButton.clicked -= OnSaveButtonClicked;       
        quitButton.clicked -= OnQuitButtonClicked;
        backButton.clicked -= OnBackButtonClicked;
        //startButton.clicked += OnPlayButtonClicked;
        startButton.UnregisterCallback<ClickEvent, int>(OnPlayButtonClicked, TrickleDown.TrickleDown);
        optionsButton.clicked += OnSettingsButtonClicked;
        exitButton.clicked += OnQuitButtonClicked;

        // dropdown specific handlers
        qualityDropdown.UnregisterValueChangedCallback(evt => RuntimeManager.PlayOneShot(dropdownSelectEvent));
        qualityDropdown.UnregisterCallback<MouseDownEvent>(OnDropdownDrop);
        

        // hover event handler
        mm.UnregisterCallback<MouseOverEvent>(OnHover, TrickleDown.TrickleDown);

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
            // adjust these to change default values
            lookSenSlider.value = 20;
            walkSpeedSlider.value = 4;
        }

    }

    private void OnHover(MouseOverEvent hov)
    {
        if (hov.target is Button)
        {
            RuntimeManager.PlayOneShot(hoverEvent);
        }
    }

    private void OnBackButtonClicked()
    { 
        //FMOD
        // Play back button sfx
        RuntimeManager.PlayOneShot(backEvent);

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
        // clear inventory on quit
        GameManager.Instance.ClearInventory();

        // FMOD
        // Play Quit SFX
        RuntimeManager.PlayOneShot(quitEvent);

        Application.Quit();
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #endif
    }

    private void OnSettingsButtonClicked()
    {
        // FMOD
        // Play settings enter SFX
        RuntimeManager.PlayOneShot(optionsEvent);

        Debug.Log("Settings");
        initialMenu.AddToClassList("remove");
        startMenu.AddToClassList("remove");
        settingsMenu.RemoveFromClassList("remove");
    }

    // This is for clicking on dropdown
    private void OnDropdownDrop(MouseDownEvent evt)
    {
        // FMOD
        // Play dropdown sfx
        RuntimeManager.PlayOneShot(dropdownEvent);
    }

    private void OnSaveButtonClicked()
    {
        Debug.Log("Save Game");
        // FMOD
        // Play save sfx
        RuntimeManager.PlayOneShot(saveEvent);

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

        // set player data
        GameManager.Instance.SetPlayerData(lookSenSlider.value, walkSpeedSlider.value);
    }

    private void OnPlayButtonClicked(ClickEvent clicky, int startOrCont)
    {
        // FMOD
        // check if start btn or continue btn
        // if start...
        if (startOrCont == 0)
        {
          RuntimeManager.PlayOneShot(continueEvent);
        } else 
        {
            RuntimeManager.PlayOneShot(startEvent);
        }
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
                        menuText.text = "You are at The Burren, where you had your first date with Alison!";
                        break;
                    case 3:
                        baseImg.image = Resources.Load<Texture2D>("TMO_pauselocation_2nd");
                        menuText.text = "You are at The Somerville Theatre, where you had your first date with Alison :)";
                        break;
                    case 4:
                        baseImg.image = Resources.Load<Texture2D>("TMO_pauselocation_3rdb");
                        menuText.text = "You are at Echo Lake, where you proposed to Alison! I wonder how you get the ring?";
                        break;
                    default:
                        baseImg.image = null;
                        menuText.text = null;
                        break;
                }

                // FMOD sound for pause menu
                RuntimeManager.PlayOneShot(pauseEvent);
                RuntimeManager.StudioSystem.setParameterByName("gamePause", 0);

                // open main menu
                mm.RemoveFromClassList("hide");
                Debug.Log("opening main menu and disabling player controls");
                inputReader.DisablePlayerControls();
            }
            else
            {
                // close main menu
                mm.AddToClassList("hide");

                // FMOD sound for pause menu
                RuntimeManager.PlayOneShot(continueEvent);
                RuntimeManager.StudioSystem.setParameterByName("gamePause", 1);

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
