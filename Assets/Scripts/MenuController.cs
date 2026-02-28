using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    [SerializeField] InputReader inputReader;

    private GameObject Inventory;
    private VisualElement invPanel;

    private SaveManager SaveManager;

    public VisualElement mm;

    public Button playButton;
    public Button settingsButton;
    public Button saveButton;
    public Button quitButton;

    void Awake()
    {
        mm = GetComponent<UIDocument>().rootVisualElement; 

        
        
    }

    void OnEnable()
    {
        // get buttons and subscribe to click events
        playButton = mm.Q<Button>("PlayButton");
        playButton.clicked += OnPlayButtonClicked;

        settingsButton = mm.Q<Button>("SettingsButton");
        settingsButton.clicked += OnSettingsButtonClicked;

        saveButton = mm.Q<Button>("SaveButton");
        saveButton.clicked += OnSaveButtonClicked;

        quitButton = mm.Q<Button>("QuitButton");
        quitButton.clicked += OnQuitButtonClicked;
 
    }

    void Start()
    {
        // get save manager
        SaveManager = GameObject.FindWithTag("SaveManager").GetComponent<SaveManager>();

        // get inventory ui
        Inventory = GameObject.FindWithTag("Inventory");
        invPanel = Inventory.GetComponent<UIDocument>().rootVisualElement;

        // subscribe to main menu toggle event
        inputReader.MainMenuToggleEvent += OnMainMenuToggle;

        // hide main menu if not first scene
        if (GameManager.Instance.GetSceneId() != 0)
        {
            mm.AddToClassList("hide");
        }

        // if first scene and we have save data
        if (GameManager.Instance.GetSceneId() == 0 && SaveManager.LoadPlayerData() != null)
        {
            playButton.text = "Continue";
        }
        else if (GameManager.Instance.GetSceneId() == 0 && SaveManager.LoadPlayerData() == null)
        {
            // we dont have save data in first scene
            playButton.text = "New Game";
        }
        else 
        {
            playButton.text = "Continue";
        }

    }

    private void OnQuitButtonClicked()
    {
        Application.Quit();
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #endif
    }

    private void OnSettingsButtonClicked()
    {
        Debug.Log("Settings");
    }

    private void OnSaveButtonClicked()
    {
        Debug.Log("Save Game");
        SaveManager.SavePlayerData();
    }

    private void OnPlayButtonClicked()
    {
        if (GameManager.Instance.GetSceneId() == 0 && playButton.text == "New Game")
        {
            GameManager.Instance.MoveToScene(1);
        }
        else if (GameManager.Instance.GetSceneId() == 0 && playButton.text == "Continue")
        {
            GameManager.Instance.MoveToScene(SaveManager.LoadPlayerData().sceneNum);
        }
        else
        {
            // hide main menu
            mm.AddToClassList("hide");

        }

        // enable player controls
        inputReader.EnablePlayerControls();
    }

    void OnMainMenuToggle(bool MainMenuToggled)
    {
        // if we toggled opened main menu with ESC and inventory isnt open
        if (MainMenuToggled && invPanel.ClassListContains("hide"))
        {
            Debug.Log("main menu toggle triggered");
            mm.ToggleInClassList("hide");

            // disable/enable player controls if main menu is on screen or not
            if (mm.ClassListContains("hide"))
            {
                inputReader.EnablePlayerControls();
            }
            else {
                inputReader.DisablePlayerControls();
            }
        }
    }
    
}
