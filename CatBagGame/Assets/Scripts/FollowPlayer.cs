using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    GameObject player;
    [SerializeField] Vector3 offset;
    [SerializeField] float minPos;
    [SerializeField] float maxPos;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerBehaviour>().gameObject;
    }

    private void LateUpdate()
    {
        //transform.position = new Vector3(player.transform.position.x, offset.y, -10);
        float newYPos = Mathf.Clamp(player.transform.position.x, minPos, maxPos);
        transform.position = new Vector3(newYPos, offset.y, -10);
    }
}
