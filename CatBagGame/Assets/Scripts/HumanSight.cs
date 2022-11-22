using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanSight : MonoBehaviour
{
    GameObject player;
    GameObject human;

    private void Start()
    {
        player = FindObjectOfType<PlayerBehaviour>().gameObject;
        human = transform.parent.gameObject;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            human.GetComponent<HumanBehaviour>().SeePlayer();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            human.GetComponent<HumanBehaviour>().StopSeeingPlayer();
        }
    }
}
