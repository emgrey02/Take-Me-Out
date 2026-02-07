using UnityEngine;

public class Base : MonoBehaviour
{
    public UIManager uiManager;

    void OnTriggerEnter(Collider player)
    {
        Debug.Log("Player entered area around base");
        uiManager.ShowPrompt("FirstScene");
    }

    void OnTriggerExit(Collider player)
    {
        Debug.Log("Player left area around base");
        uiManager.HidePrompt("FirstScene");
    }
}
