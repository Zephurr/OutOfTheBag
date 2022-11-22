using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clothes : InteractableObject
{
    [SerializeField] float spriteXOffset;
    GameObject player;
    bool isGrabbed = false;

    private void Start()
    {
        player = FindObjectOfType<PlayerBehaviour>().gameObject;
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
}
