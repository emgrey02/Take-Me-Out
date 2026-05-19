using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }


    void Update()
    {
        transform.position = player.transform.position + new Vector3(0, .75f, 0);
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, player.transform.eulerAngles.y, transform.rotation.eulerAngles.z));
    }
}
