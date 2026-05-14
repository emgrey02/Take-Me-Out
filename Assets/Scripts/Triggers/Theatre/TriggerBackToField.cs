using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Playables;

public class TriggerBackToField:MonoBehaviour
{
    void OnTriggerEnter(Collider player)
    {
        Debug.Log("Going back to baseball field");
        GameManager.Instance.MoveToScene(1);
    }
}
