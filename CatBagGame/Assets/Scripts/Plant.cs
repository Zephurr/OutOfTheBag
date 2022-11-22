using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : InteractableObject
{
    [SerializeField] GameObject particles;
    [SerializeField] Vector3 offset;
    GameManager gm;

    private void Start()
    {
        gm = FindObjectOfType <GameManager>();
    }

    public override void Interact()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        Instantiate(particles, transform.position + offset, Quaternion.identity);

        gm.UpdateScore();

        base.Interact();
    }
}
