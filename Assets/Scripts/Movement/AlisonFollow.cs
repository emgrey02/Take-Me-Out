using UnityEngine;

public class AlisonFollow : MonoBehaviour
{
    private Transform player;
    public bool lake;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        transform.rotation= new Quaternion(0f, 0f, 0f, 0f);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (lake) {
            transform.position = player.transform.position + new Vector3(-.7f, -1f, .7f);
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, -player.transform.eulerAngles.y + 90, transform.rotation.eulerAngles.z));
        } else {
            transform.position = player.transform.position + new Vector3(.5f, -1f, -.5f);
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, -player.transform.eulerAngles.y + 90, transform.rotation.eulerAngles.z));
        }
    }
}
