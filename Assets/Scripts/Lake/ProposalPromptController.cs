using UnityEngine;

public class ProposalPromptController : MonoBehaviour
{
    public GameObject proposalText;
    public bool promptTriggered = false;
    public bool promptIsShowing = false;

    private static ProposalPromptController _instance;

    public static ProposalPromptController Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public bool PromptIsShowing()
    {
        return _instance.promptIsShowing;
    }

    public void TriggerProposalPrompt()
    {
        if (!promptTriggered)
        {
            promptTriggered = true;
        }
    }

    public void ActivateProposalText()
    {
        if (promptTriggered)
        {
            proposalText.SetActive(true);
        }
        
    }

    public void DeactivateProposalText()
    {
        if (promptTriggered)
        {
            proposalText.SetActive(false);
        }
    }

        // Update is called once per frame
    void Update()
    {
        if (proposalText.activeSelf)
        {
            promptIsShowing = true;
        } else
        {
            promptIsShowing = false;
        }
    }
}
