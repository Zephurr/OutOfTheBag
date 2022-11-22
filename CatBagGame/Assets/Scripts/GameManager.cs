using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private ParticleSystem scoreParticles;

    private int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        scoreParticles = GameObject.Find("ScoreGainParticles").GetComponent<ParticleSystem>();
        scoreText.text = "Mischievous Deeds: " + score;
    }

    public void UpdateScore()
    {
        score += 1;
        scoreText.text = "Mischievous Deeds: " + score;
        scoreParticles.Play();
    }
}
