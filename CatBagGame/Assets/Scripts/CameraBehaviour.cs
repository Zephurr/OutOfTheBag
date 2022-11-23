using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] List<float> xPoints;
    [SerializeField] int curXPointIndex;
    [SerializeField] float lerpSpeed;
    bool lerping = false;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        curXPointIndex = xPoints.IndexOf(Camera.main.transform.position.x);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !lerping)
        {
            player = collision.gameObject;
            Debug.Log("camera move");
            if (collision.gameObject.transform.position.x < Camera.main.transform.position.x && curXPointIndex != 0)
            {
                Debug.Log("camera move left");
                StartCoroutine(LerpToPoint(Camera.main.transform.position, new Vector3(xPoints[curXPointIndex - 1], Camera.main.transform.position.y, Camera.main.transform.position.z)));
            }
            else if (collision.gameObject.transform.position.x > Camera.main.transform.position.x && curXPointIndex != xPoints.Count - 1)
            {
                Debug.Log("camera move right");
                StartCoroutine(LerpToPoint(Camera.main.transform.position, new Vector3(xPoints[curXPointIndex + 1], Camera.main.transform.position.y, Camera.main.transform.position.z)));
            }
        }
    }

    IEnumerator LerpToPoint(Vector3 startPoint, Vector3 newPoint)
    {
        Debug.Log("camera lerp");

        player.GetComponent<PlayerBehaviour>().Animator.SetBool("walk", true);

        lerping = true;
        float timeElapsed = 0;
        while (timeElapsed < lerpSpeed)
        {
            timeElapsed += Time.deltaTime;
            Camera.main.transform.position = Vector3.Lerp(startPoint, newPoint, timeElapsed / lerpSpeed);
            player.transform.Translate((player.GetComponent<PlayerBehaviour>().Speed / 2) * Time.deltaTime, 0, 0);
            
            yield return null;
        }
        transform.position = newPoint;
        curXPointIndex = xPoints.IndexOf(newPoint.x);
        player.GetComponent<PlayerBehaviour>().Animator.SetBool("walk", false);
        lerping = false;
    }
}
