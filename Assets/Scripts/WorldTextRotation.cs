using UnityEngine;
using TMPro;

public class WorldTextRotation : MonoBehaviour
{
    public TextMeshProUGUI text;

    // Update is called once per frame
    void Update()
    {
        text.transform.rotation = Camera.main.transform.rotation;
    }
}
