using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class WeddingSceneController : MonoBehaviour
{
    public GameObject firstDollyCamera;
    public GameObject skyCamera;

    public GameObject Alison;
    public GameObject Griffin;

    public GameObject heartParticles;

    public GameObject takeMeOutLogo;

    public GameObject credits;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        firstDollyCamera.SetActive(false);
        //skyCamera.SetActive(false);
        Griffin.SetActive(false);

        StartCoroutine(StartFirstDolly());
    }

    IEnumerator StartFirstDolly()
    {
        yield return new WaitForSeconds(2.8f);
        firstDollyCamera.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
 
        if (firstDollyCamera.GetComponent<CinemachineSplineDolly>().CameraPosition >= 1)
        {
            if (firstDollyCamera.activeSelf == true)
            {
                heartParticles.SetActive(true);
                firstDollyCamera.SetActive(false);
                Griffin.SetActive(true);
                skyCamera.SetActive(true);
                Griffin.GetComponent<Animator>().SetBool("turn", true);
                Alison.GetComponent<Animator>().SetBool("turn", true);
            }
        }

        if (skyCamera.GetComponent<CinemachineSplineDolly>().CameraPosition >= 1 && !credits.activeSelf)
        {
            takeMeOutLogo.SetActive(true);
            StartCoroutine(ShowCredits());
        }
    }

    IEnumerator ShowCredits()
    {
        yield return new WaitForSeconds(5f);
        // Show credits here
        takeMeOutLogo.SetActive(false);
        credits.SetActive(true);
    }
}
