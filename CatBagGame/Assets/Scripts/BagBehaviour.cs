using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagBehaviour : InteractableObject
{
    GameObject player;
    [SerializeField] private KeyCode outOfBagKeyCode;
    bool inBag = false;

    [SerializeField] private Vector3 posOffset;
    [SerializeField] private float spriteXOffset;

    private void Start()
    {
        player = FindObjectOfType<PlayerBehaviour>().gameObject;
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(outOfBagKeyCode))
        {
            OutOfBag();
        }

        if (inBag)
        {
            if (transform.position != player.transform.position + posOffset)
            {
                transform.position = player.transform.position + posOffset;
            }
            if (!(player.GetComponent<Rigidbody2D>().constraints == RigidbodyConstraints2D.FreezePositionY))
            {

            }
        }
    }

    public override void Interact()
    {
        transform.parent = player.transform;
        transform.localPosition = new Vector3(0, transform.localPosition.y);
        if (player.GetComponent<PlayerBehaviour>().FacingRight)
        {
            spriteXOffset = Mathf.Abs(spriteXOffset);
        }
        else
        {
            spriteXOffset *= -1;
        }
        transform.GetChild(0).transform.localPosition = new Vector3(transform.localPosition.x + spriteXOffset, 0);
        inBag = true;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

        hasBeenInteracted = true;
    }

    void OutOfBag()
    {
        transform.parent = null;
        hasBeenInteracted = false;
        inBag = false;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
