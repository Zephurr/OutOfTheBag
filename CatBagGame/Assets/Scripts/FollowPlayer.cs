using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    GameObject player;
    [SerializeField] Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerBehaviour>().gameObject;
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x, offset.y, -10);
    }
}
