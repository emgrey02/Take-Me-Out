using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WorldTextRotation : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image img;

    // Update is called once per frame
    void Update()
    {
        if (text)
        {
            text.transform.rotation = Camera.main.transform.rotation;

        }
        if (img)
        {
            img.transform.rotation = Camera.main.transform.rotation;

        }
    }
}
