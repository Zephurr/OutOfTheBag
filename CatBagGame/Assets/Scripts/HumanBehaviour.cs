using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum FacingDirection
{
    Right, Left
}

public class HumanBehaviour : MonoBehaviour
{
    GameObject head;
    GameObject sightRange;
    GameObject neck;
    GameObject player;

    [SerializeField] FacingDirection curFd;

    Vector3 startPos;

    [SerializeField] float chaseSpeed;
    [SerializeField] bool caughtPlayer = false;
    [SerializeField] bool chasingPlayer = false;

    [SerializeField] bool seeingPlayer = false;
    [SerializeField] bool canSeePlayer = true;

    [SerializeField] float headRotateOffset;

    [SerializeField] float playerSightStay = 0;
    [SerializeField] private float playerSightLimit;

    bool catchPlayer = true;

    Animator animator;

    public bool SeeingPlayer { get => seeingPlayer; set => seeingPlayer = value; }
    public bool CanSeePlayer { get => canSeePlayer; set => canSeePlayer = value; }

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        sightRange = transform.GetChild(1).gameObject;
        neck = transform.GetChild(0).gameObject;
        head = neck.transform.GetChild(0).gameObject;
        player = FindObjectOfType<PlayerBehaviour>().gameObject;
        animator = GetComponent<Animator>();
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

        /*if (GetComponent<Patrol>().MoveToggle)
        {
            neck.transform.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            neck.transform.transform.rotation = Quaternion.Euler(0, 180, angle + headRotateOffset);
        }*/

        if (curFd == FacingDirection.Left)
        {
            neck.transform.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            neck.transform.transform.rotation = Quaternion.Euler(180, 0, -angle);
        }

        //GetComponent<Patrol>().unPaused = false;

        if (player.GetComponent<PlayerBehaviour>().IsMoving)
        {
            playerSightStay += Time.deltaTime;
        }
        if (playerSightStay >= playerSightLimit)
        {
           //Debug.Log("Cat is out of the bag!");
            //GetComponent<Patrol>().StopAllCoroutines();
            chasingPlayer = true;
            animator.SetBool("Walking", true);
            ChasePlayer();
        }
    }

    public void StopSeeingPlayer()
    {
        animator.SetBool("Petting", false);

        seeingPlayer = false;
        //GetComponent<Patrol>().HitObstacle = false;
        CanSeePlayer = true;

        //FindObjectOfType<CameraBehaviour>().FollowPlayer = false;
        //FindObjectOfType<CameraBehaviour>().SnapToPosition();

        neck.transform.localRotation = Quaternion.Euler(0, 0, 0);

        playerSightStay = 0;

        //GetComponent<Patrol>().unPaused = true;
        Debug.Log("Stop seeing player");
        if (chasingPlayer)
        {
            chasingPlayer = false;
            catchPlayer = true;
            StartCoroutine(ReturnToStart());
        }
    }

    public void ToggleFacingDirection()
    {
        if (curFd == FacingDirection.Right)
        {
            curFd = FacingDirection.Left;
        }
        else
        {
            curFd = FacingDirection.Right;
        }
    }

    IEnumerator ReturnToStart()
    {
        transform.Rotate(0, 180, 0);
        ToggleFacingDirection();
        //GetComponent<Patrol>().MoveToggle = !//GetComponent<Patrol>().MoveToggle;
        Vector3 pos = transform.position;
        Debug.Log("return to start");
        float timeElapsed = 0;

        //Vector3 newPos = new Vector3(startPos.x, transform.position.y, transform.position.z);
        while (timeElapsed < (chaseSpeed * 2))
        {
            timeElapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(pos, startPos, timeElapsed / (chaseSpeed * 2));

            yield return null;
        }
        transform.position = startPos;

        animator.SetBool("Walking", false);

        //Debug.Log("patrol");
        transform.Rotate(0, 180, 0);
        ToggleFacingDirection();
        //GetComponent<Patrol>().MoveToggle = !//GetComponent<Patrol>().MoveToggle;
        //GetComponent<Patrol>().StartCoroutine(//GetComponent<Patrol>().LerpToPoint(//GetComponent<Patrol>().PointB));
        CanSeePlayer = true;
    }

    public void ChasePlayer()
    {
        //FindObjectOfType<CameraBehaviour>().FollowPlayer = true;
        if (!caughtPlayer)
        {
            Vector3 target = new Vector3(player.transform.position.x, transform.position.y);
            transform.position = Vector3.MoveTowards(transform.position, target, chaseSpeed * Time.deltaTime);
        }
        else
        {
            //Debug.Log("Caught player!");

            if (catchPlayer)
            {
                Debug.Log("Catch player");
                CatchPlayer();
            }

            if (player.GetComponent<PlayerBehaviour>().Escaped && CanSeePlayer)
            {
                CanSeePlayer = false;
                caughtPlayer = false;
                StopSeeingPlayer();
            }
        }        
    }

    public void CatchPlayer()
    {
        catchPlayer = false;
        player.GetComponent<PlayerBehaviour>().CanMove = false;

        animator.SetBool("Walking", false);
        animator.SetBool("Petting", true);
        player.GetComponent<PlayerBehaviour>().EscapePets();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && chasingPlayer)
        {
            caughtPlayer = true;
        }
    }
}
