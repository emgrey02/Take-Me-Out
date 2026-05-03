using UnityEngine;

public class AlisonFollow : MonoBehaviour
{
    private Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        transform.rotation= new Quaternion(0f,-90f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + new Vector3(.5f, -1f, -.5f);
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, -player.transform.eulerAngles.y, transform.rotation.eulerAngles.z));
    }
}
