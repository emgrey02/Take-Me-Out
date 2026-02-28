using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    [SerializeField] InputReader inputReader;

    private GameObject Inventory;
    private VisualElement invPanel;

    public VisualElement ui;

    public Button continueButton;
    public Button graphicsButton;
    public Button audioButton;
    public Button quitButton;

    void Awake()
    {
        ui = GetComponent<UIDocument>().rootVisualElement;
    }

    void OnEnable()
    {
        continueButton = ui.Q<Button>("ContinueButton");
        continueButton.clicked += OnContinueButtonClicked;

        graphicsButton = ui.Q<Button>("GraphicsButton");
        graphicsButton.clicked += OnGraphicsButtonClicked;

        audioButton = ui.Q<Button>("AudioButton");
        audioButton.clicked += OnAudioButtonClicked;

        quitButton = ui.Q<Button>("QuitButton");
        quitButton.clicked += OnQuitButtonClicked;
    }

    void Start()
    {
        Inventory = GameObject.FindWithTag("Inventory");
        inputReader.MainMenuToggleEvent += OnMainMenuToggle;
        ui.AddToClassList("hide");
    }

    private void OnQuitButtonClicked()
    {
        Application.Quit();
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #endif
    }

    private void OnGraphicsButtonClicked()
    {
        Debug.Log("Graphics Options");
    }

    private void OnAudioButtonClicked()
    {
        Debug.Log("Audio Options");
    }

    private void OnContinueButtonClicked()
    {
        Debug.Log("Disabling Main Menu");
        ui.AddToClassList("hide");
        inputReader.EnablePlayerControls();
    }

    void OnMainMenuToggle(bool MainMenuToggled)
    {
        if (MainMenuToggled && ui.ClassListContains("hide"))
        {
            Debug.Log("main menu toggle triggered");
            ui.ToggleInClassList("hide");

            // disable/enable player controls if inventory on screen or not
            if (ui.ClassListContains("hide"))
            {
                inputReader.EnablePlayerControls();
            }
            else {
                inputReader.DisablePlayerControls();
            }
        }
    }
    
}
