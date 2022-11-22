using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanBehaviour : MonoBehaviour
{
    GameObject head;
    GameObject sightRange;
    GameObject neck;
    GameObject player;

    [SerializeField] float chaseSpeed;
    [SerializeField] bool caughtPlayer = false;
    [SerializeField] bool chasingPlayer = false;

    [SerializeField] bool seeingPlayer = false;

    [SerializeField] float headRotateOffset;

    [SerializeField] float playerSightStay = 0;
    [SerializeField] private float playerSightLimit;

    public bool SeeingPlayer { get => seeingPlayer; set => seeingPlayer = value; }

    // Start is called before the first frame update
    void Start()
    {
        sightRange = transform.GetChild(1).gameObject;
        neck = transform.GetChild(0).gameObject;
        head = neck.transform.GetChild(0).gameObject;
        player = FindObjectOfType<PlayerBehaviour>().gameObject;
    }

    public void SeePlayer()
    {
        if (!seeingPlayer)
        {
            seeingPlayer = true;
        }

        // get the x and y distances between the tank and the mouse position
        Vector2 delta = transform.position - player.transform.position;
        // using these two sides of a figurative triangle, use tangent to calculate the angle between them,
        // then convert that angle to degrees
        float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;

        if (GetComponent<Patrol>().MoveToggle)
        {
            neck.transform.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            neck.transform.transform.rotation = Quaternion.Euler(0, 0, angle + headRotateOffset);
        }

        GetComponent<Patrol>().unPaused = false;

        if (player.GetComponent<PlayerBehaviour>().IsMoving)
        {
            playerSightStay += Time.deltaTime;
        }
        if (playerSightStay >= playerSightLimit)
        {
            Debug.Log("Cat is out of the bag!");
            GetComponent<Patrol>().StopAllCoroutines();
            chasingPlayer = true;
            ChasePlayer();
        }
    }

    public void StopSeeingPlayer()
    {
        seeingPlayer = false;

        neck.transform.transform.rotation = Quaternion.Euler(0, 0, 0);

        playerSightStay = 0;

        GetComponent<Patrol>().unPaused = true;
        Debug.Log("Stop seeing player");
        if (chasingPlayer)
        {
            chasingPlayer = false;
            StartCoroutine(ReturnToStart());
        }
    }

    IEnumerator ReturnToStart()
    {
        Vector3 pos = transform.position;
        Debug.Log("return to start");
        float timeElapsed = 0;
        while (timeElapsed < (chaseSpeed * 2))
        {
            timeElapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(pos, GetComponent<Patrol>().StartPoint, timeElapsed / (chaseSpeed * 2));

            yield return null;
        }
        transform.position = GetComponent<Patrol>().StartPoint;

        Debug.Log("patrol");
        GetComponent<Patrol>().MoveToggle = true;
        GetComponent<Patrol>().StartCoroutine(GetComponent<Patrol>().LerpToPoint(GetComponent<Patrol>().Target.transform.position));
    }

    public void ChasePlayer()
    {
        if (!caughtPlayer)
        {
            Vector3 target = new Vector3(player.transform.position.x, transform.position.y);
            transform.position = Vector3.MoveTowards(transform.position, target, chaseSpeed * Time.deltaTime);
        }
        else
        {
            Debug.Log("Caught player!");

            player.GetComponent<PlayerBehaviour>().CanMove = false;
            player.GetComponent<PlayerBehaviour>().EscapePets();
        }        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && chasingPlayer)
        {
            caughtPlayer = true;
        }
    }
}
