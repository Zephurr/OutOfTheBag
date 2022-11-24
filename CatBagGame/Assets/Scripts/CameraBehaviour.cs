using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Direction
{
    Left, Right
}

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] List<float> xPoints;
    [SerializeField] int curXPointIndex;
    [SerializeField] float lerpSpeed;
    bool lerping = false;

    Direction curDirection;

    GameObject player;

    [SerializeField] bool followPlayer = false;
    [SerializeField] Vector3 offset;

    public bool FollowPlayer { get => followPlayer; set => followPlayer = value; }

    // Start is called before the first frame update
    void Start()
    {
        curXPointIndex = xPoints.IndexOf(Camera.main.transform.position.x);
        player = FindObjectOfType<PlayerBehaviour>().gameObject;
    }

    private void Update()
    {
        if (followPlayer)
        {
            transform.parent.transform.position = new Vector3(player.transform.position.x, offset.y, -10);
            GetComponent<Collider2D>().isTrigger = true;
        }
    }

    public void SnapToPosition()
    {
        GetComponent<Collider2D>().isTrigger = false;

        float min = float.MaxValue;
        float closestPoint = 0;
        foreach (float point in xPoints)
        {
            float dis = Vector2.Distance(Camera.main.transform.position, new Vector2(point, Camera.main.transform.position.y));
            if (dis < min)
            {
                min = dis;
                closestPoint = point;
            }
        }
        StartCoroutine(LerpToPoint(Camera.main.transform.position, new Vector3(closestPoint, Camera.main.transform.position.y, Camera.main.transform.position.z), false));        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !lerping)
        {
            Debug.Log("camera move");
            if (collision.gameObject.transform.position.x < Camera.main.transform.position.x && curXPointIndex != 0)
            {
                Debug.Log("camera move left");
                curDirection = Direction.Left;
                StartCoroutine(LerpToPoint(Camera.main.transform.position, new Vector3(xPoints[curXPointIndex - 1], Camera.main.transform.position.y, Camera.main.transform.position.z), true));
            }
            else if (collision.gameObject.transform.position.x > Camera.main.transform.position.x && curXPointIndex != xPoints.Count - 1)
            {
                Debug.Log("camera move right");
                curDirection = Direction.Right;
                StartCoroutine(LerpToPoint(Camera.main.transform.position, new Vector3(xPoints[curXPointIndex + 1], Camera.main.transform.position.y, Camera.main.transform.position.z), true));
            }
        }
    }

    IEnumerator LerpToPoint(Vector3 startPoint, Vector3 newPoint, bool playerStopMoving)
    {
        Debug.Log("camera lerp");

        if (playerStopMoving)
        {
            player.GetComponent<PlayerBehaviour>().Animator.SetBool("walk", true);
            player.GetComponent<PlayerBehaviour>().enabled = false;
        }

        float speed;
        if (curDirection == Direction.Right)
        {
            speed = 2f;
        }
        else
        {
            speed = -2f;
        }

        lerping = true;
        float timeElapsed = 0;
        while (timeElapsed < lerpSpeed)
        {
            timeElapsed += Time.deltaTime;
            Camera.main.transform.position = Vector3.Lerp(startPoint, newPoint, timeElapsed / lerpSpeed);
            //player.transform.Translate(speed * Time.deltaTime, 0, 0);

            if (playerStopMoving)
            {
                player.transform.position = new Vector3(player.transform.position.x + speed * Time.deltaTime, player.transform.position.y, player.transform.position.z);
            }
            yield return null;
        }
        transform.position = newPoint;
        curXPointIndex = xPoints.IndexOf(newPoint.x);

        if (playerStopMoving)
        {
            player.GetComponent<PlayerBehaviour>().Animator.SetBool("walk", false);
            player.GetComponent<PlayerBehaviour>().enabled = true;
        }
        lerping = false;
    }
}
