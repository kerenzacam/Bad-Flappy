using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Put this on any GameObject in the scene (e.g. the PipeSpawner or a "Score" object).
// Drag the Bird into "bird" and the PipeSpawner into "pipeScript" in the Inspector.
public class ScoreScript : MonoBehaviour
{
    public BirdScript bird;
    public PipeScript pipeScript;
    public TMP_Text scoreText; // optional: a TextMeshPro text to show the score

    public int score = 0;

    // Pipes the bird has already scored on, so each pipe only counts once
    private readonly HashSet<Transform> scoredPipes = new HashSet<Transform>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = 0;
        UpdateScoreText();
    }

    // Update is called once per frame
    void Update()
    {
        if (bird == null || pipeScript == null) return;
        if (!bird.isAlive)
        {
            // No more points after dying, show the game over message instead
            if (scoreText != null)
            {
                scoreText.text = "Game Over - Score: " + score + "\nPress R to restart";
            }
            return;
        }

        float birdX = bird.transform.position.x;

        foreach (Transform pipe in pipeScript.Pipes)
        {
            // When a pipe moves past the bird, the bird has gone through the gap
            if (pipe.position.x < birdX && !scoredPipes.Contains(pipe))
            {
                scoredPipes.Add(pipe);
                AddScore(1);
            }
        }

        // Forget pipes that have been destroyed
        scoredPipes.RemoveWhere(p => p == null);
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
        Debug.Log("Score: " + score);
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }
}
