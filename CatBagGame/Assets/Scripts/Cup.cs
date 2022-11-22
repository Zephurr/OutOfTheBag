using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cup : MonoBehaviour
{
    [SerializeField] GameObject waterParticles;
    bool hasBeenKnockedOver = false;

    GameManager gm;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Platform") && !hasBeenKnockedOver)
        {
            Physics2D.IgnoreCollision(transform.parent.GetComponent<Collider2D>(),
                FindObjectOfType<PlayerBehaviour>().GetComponent<Collider2D>());
            GameObject waterPrefab = Instantiate(waterParticles, transform.position, Quaternion.identity);
            waterPrefab.transform.up = transform.up;

            gm.UpdateScore();
            hasBeenKnockedOver = true;
        }
    }
}
