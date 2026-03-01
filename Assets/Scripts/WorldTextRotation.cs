using UnityEngine;
using TMPro;

public class WorldTextRotation : MonoBehaviour
{
    public TextMeshProUGUI text;

    // Update is called once per frame
    // late update to match camera movement
    void LateUpdate()
    {
        text.transform.rotation = Camera.main.transform.rotation;
    }
}
