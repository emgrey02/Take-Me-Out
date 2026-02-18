using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    public InputAction mainmenuToggle;

    public VisualElement ui;

    public Button continueButton;
    public Button graphicsButton;
    public Button audioButton;
    public Button quitButton;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
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
        mainmenuToggle = InputSystem.actions.FindAction("MainMenuToggle");
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
    }

    void Update()
    {
        if (mainmenuToggle.triggered)
        {
            Debug.Log("main menu toggle triggered");
            ui.ToggleInClassList("hide");
        }
    }
    
}
