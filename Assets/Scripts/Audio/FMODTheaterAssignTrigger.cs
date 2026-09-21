using UnityEngine;

public class FMODTheaterAssignTrigger : MonoBehaviour
{

    public bool isEntering;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isEntering = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeState()
    {
        if (isEntering)
        {
            isEntering = false;
        } else
        {
            isEntering = true;
        }
    }
}
