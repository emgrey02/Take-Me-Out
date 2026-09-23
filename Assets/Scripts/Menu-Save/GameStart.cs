using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class GameStart : MonoBehaviour
{

    public GameObject logo;
    public GameObject text;

    private VisualElement mm;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text.SetActive(false);
        logo.SetActive(true);
        mm = GameObject.FindWithTag("MainMenu").GetComponent<UIDocument>().rootVisualElement;
        mm.AddToClassList("hide");
        StartCoroutine(NextSlide(false));
    }

    IEnumerator NextSlide(bool menu)
    {
        Debug.Log("waiting 6 sec for next slide");
        yield return new WaitForSeconds(6f);

        if (menu)
        {
            Debug.Log("switching to menu");
            text.SetActive(false);
            mm.RemoveFromClassList("hide");
        } else
        {
            Debug.Log("switching to text");
            logo.SetActive(false);
            text.SetActive(true);
            StartCoroutine(NextSlide(true));
        }
    }
}
