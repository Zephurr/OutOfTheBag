using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanBody : InteractableObject
{
    HumanBehaviour hb;
    [SerializeField] float jumpscareCooldownSeconds;
    bool canBeJumpscared = true;
    GameManager gm;

    private void Start()
    {
        hb = transform.parent.gameObject.GetComponent<HumanBehaviour>();
        gm = FindObjectOfType<GameManager>();
    }

    public override void Interact()
    {
        if (!hb.SeeingPlayer && canBeJumpscared)
        {
            Debug.Log("jumpscare human");
            hb.GetComponent<Animator>().SetBool("Scared", true);
            gm.UpdateScore();

            StartCoroutine(JumpscareCooldown());
        }
    }

    IEnumerator JumpscareCooldown()
    {
        canBeJumpscared = false;
        hb.gameObject.transform.Rotate(0, 180, 0);
        hb.ToggleFacingDirection();
        //hb.GetComponent<Patrol>().MoveToggle = !hb.GetComponent<Patrol>().MoveToggle;
        hb.SeePlayer();
        yield return new WaitForSecondsRealtime(jumpscareCooldownSeconds);
        hb.GetComponent<Animator>().SetBool("Scared", false);
        hb.gameObject.transform.Rotate(0, 180, 0);
        hb.ToggleFacingDirection();
    }
}
