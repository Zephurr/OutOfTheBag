using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clothes : InteractableObject
{
    [SerializeField] float spriteXOffset;
    [SerializeField] float spriteYOffset;
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

            transform.parent.parent = player.transform;
            player.GetComponent<PlayerBehaviour>().CanSwitchDirection = false;
            player.GetComponent<PlayerBehaviour>().Speed = player.GetComponent<PlayerBehaviour>().CrouchingSpeed;

            transform.position = new Vector3(player.transform.position.x + spriteXOffset, player.transform.position.y + spriteYOffset);

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
        transform.parent.parent = null;
        player.GetComponent<PlayerBehaviour>().CanSwitchDirection = true;
        player.GetComponent<PlayerBehaviour>().Speed = player.GetComponent<PlayerBehaviour>().WalkingSpeed;

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
