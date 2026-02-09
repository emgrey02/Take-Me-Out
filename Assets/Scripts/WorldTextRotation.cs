using UnityEngine;
using TMPro;

public class WorldTextRotation : MonoBehaviour
{
    public TextMeshProUGUI text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.transform.rotation = Camera.main.transform.rotation;
    }
}
