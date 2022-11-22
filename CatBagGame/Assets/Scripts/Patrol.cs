/*****************************************************************************
// File Name :         Patrol.cs
// Author :            Zayden Joyner
// Creation Date :     October 1, 2022
//
// Brief Description : This script allows an object to patrol back and forth indefinitely.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    // Zayden: toggle for which direction the ghost moves
    [SerializeField] private bool moveToggle = true;
    // Zayden: the point the ghost starts at (one end of the patrol)
    [SerializeField] private Vector3 startPoint;
    // Zayden: speed of movement
    [SerializeField] private float speed = 5;
    // Zayden: the other end of the patrol, based on an adjustable target gameobject
    [SerializeField] private GameObject target;

    // obstacle collision managers
    [SerializeField] bool hitObstacle = false;
    [SerializeField] Vector3 newPoint = new Vector3();
    bool gotNewSpeed = false;
    float percent;
    public bool unPaused = true;

    // the points to be lerped between
    [SerializeField] Vector3 pointB;
    [SerializeField] Vector3 pointA;

    public bool MoveToggle { get => moveToggle; set => moveToggle = value; }
    public Vector3 StartPoint { get => startPoint; set => startPoint = value; }
    public GameObject Target { get => target; set => target = value; }
    public Vector3 PointA { get => pointA; set => pointA = value; }
    public Vector3 PointB { get => pointB; set => pointB = value; }
    public bool HitObstacle { get => hitObstacle; set => hitObstacle = value; }

    /// <summary>
    /// Zayden: start the patrol coroutine and get the start point
    /// </summary>
    void Start()
    {
        startPoint = transform.position;
        StartCoroutine(LerpToPoint(target.transform.position));
    }

    private void Update()
    {
        if (GetComponent<HumanBehaviour>().SeeingPlayer)
        {
            // change the coroutine functionality
            hitObstacle = true;
            newPoint = transform.position;
        }
    }

    /// <summary>
    /// Zayden: LerpToPoint lerps the position of the ghost from one point to another based on speed
    /// </summary>
    /// <param name="targetPoint"> the initial point to be lerped to </param>
    /// <returns> null </returns>
    public IEnumerator LerpToPoint(Vector3 targetPoint)
    {
        // save the speed variable in case the ghost runs into a box
        float initialSpeed = speed;

        // run the loop forever (currently)
        while (true)
        {
            // set/reset the time elapsed to zero
            float timeElapsed = 0;

            // switch pointA and pointB depending on which direction the ghost should be moving
            if (moveToggle)
            {
                pointA = startPoint;
                pointB = targetPoint;
            }
            else
            {
                pointA = targetPoint;
                pointB = startPoint;
            }

            // lerp the position over time based on speed
            while (timeElapsed < speed)
            {      
                // if the ghost hits an obstacle...
                if (hitObstacle)
                {
                    if (unPaused)
                    {
                        // change the speed of the lerp so the ghost moves at the same rate as before
                        if (!gotNewSpeed)
                        {
                            // reset timeElapsed
                            timeElapsed = 0;

                            // calculate what percent of the original distance the new distance is
                            float fullDistance = Vector3.Distance(pointA, pointB);
                            float newDistance = Vector3.Distance(newPoint, pointB);
                            percent = newDistance / fullDistance;

                            // adjust speed according to that percent
                            speed *= percent;

                            gotNewSpeed = true;
                        }
                        timeElapsed += Time.deltaTime;
                        // change the lerp points and continue moving
                        transform.position = Vector3.Lerp(newPoint, pointB, timeElapsed / speed);
                    }
                    else
                    {
                        yield return null;
                    }
                }
                else
                {
                    timeElapsed += Time.deltaTime;
                    transform.position = Vector3.Lerp(pointA, pointB, timeElapsed / speed);
                }
                yield return null;
            }

            // rotate the object to make it look like the object is turning around to go the other direction
            transform.Rotate(0, 180, 0);

            if (hitObstacle)
            {
                // reset variables
                gotNewSpeed = false;
                speed = initialSpeed;
                hitObstacle = false;
                moveToggle = !moveToggle;
            }
            else
            {
                // toggle the move direction
                moveToggle = !moveToggle;
            }
        }
    }
}

