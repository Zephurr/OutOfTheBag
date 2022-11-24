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

    // Start is called before the first frame update
    void Start()
    {
        curXPointIndex = xPoints.IndexOf(Camera.main.transform.position.x);
        player = FindObjectOfType<PlayerBehaviour>().gameObject;
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
                StartCoroutine(LerpToPoint(Camera.main.transform.position, new Vector3(xPoints[curXPointIndex - 1], Camera.main.transform.position.y, Camera.main.transform.position.z)));
            }
            else if (collision.gameObject.transform.position.x > Camera.main.transform.position.x && curXPointIndex != xPoints.Count - 1)
            {
                Debug.Log("camera move right");
                curDirection = Direction.Right;
                StartCoroutine(LerpToPoint(Camera.main.transform.position, new Vector3(xPoints[curXPointIndex + 1], Camera.main.transform.position.y, Camera.main.transform.position.z)));
            }
        }
    }

    IEnumerator LerpToPoint(Vector3 startPoint, Vector3 newPoint)
    {
        Debug.Log("camera lerp");
        player.GetComponent<PlayerBehaviour>().Animator.SetBool("walk", true);

        player.GetComponent<PlayerBehaviour>().enabled = false;

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

            player.transform.position = new Vector3(player.transform.position.x + speed * Time.deltaTime, player.transform.position.y, player.transform.position.z);
            
            yield return null;
        }
        transform.position = newPoint;
        curXPointIndex = xPoints.IndexOf(newPoint.x);
        player.GetComponent<PlayerBehaviour>().Animator.SetBool("walk", false);

        player.GetComponent<PlayerBehaviour>().enabled = true;
        lerping = false;
    }
}
