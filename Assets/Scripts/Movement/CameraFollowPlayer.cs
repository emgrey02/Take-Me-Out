using UnityEngine;

// script to make the Camera stick to the player, around eye height, and rotate with the player on the y axis
public class CameraFollowPlayer : MonoBehaviour
{
    private Transform player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }


    void Update()
    {
        // set camera position to player position + offset for eye height
        transform.position = player.transform.position + new Vector3(0, .75f, 0);

        // set camera rotation to match player rotation on y axis (so it turns left/right with the player)
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, player.transform.eulerAngles.y, transform.rotation.eulerAngles.z));
    }
}
