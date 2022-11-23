using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clothes : InteractableObject
{
    [SerializeField] float spriteXOffset;
    GameObject player;
    GameManager gm;
    bool isGrabbed = false;
    bool canScore = true;

    private void Start()
    {
        player = FindObjectOfType<PlayerBehaviour>().gameObject;
        gm = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (isGrabbed)
        {
            if (player.GetComponent<PlayerBehaviour>().FacingRight && spriteXOffset != Mathf.Abs(spriteXOffset))
            {
                spriteXOffset = Mathf.Abs(spriteXOffset);
            }
            else if (!player.GetComponent<PlayerBehaviour>().FacingRight && spriteXOffset > 0)
            {
                spriteXOffset *= -1;
            }
            transform.position = new Vector3(player.transform.position.x + spriteXOffset, transform.position.y);

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                LetGo();
            }
        }
    }

    public override void Interact()
    {
        isGrabbed = true;

        base.Interact();
    }

    void LetGo()
    {
        isGrabbed = false;

        hasBeenInteracted = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("UnderObject") && canScore)
        {
            gm.UpdateScore();
            canScore = false;
        }
    }
}
