using UnityEngine;

public class AnimationEnd : MonoBehaviour
{

    [SerializeField] InputReader inputReader;
    public void DeactivateObject()
    {
        // remove the splash screen
        gameObject.SetActive(false);
    }

    public void AllowPlayerMovement()
    {
        inputReader.EnablePlayerControls();
    }

    public void DisablePlayerMovement()
    {
        inputReader.DisablePlayerControls();
    }
}
