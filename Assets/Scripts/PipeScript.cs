using System.Collections.Generic;
using UnityEngine;

// Put this on one empty GameObject in the scene, not on the pipe itself.
// It spawns the pipes, moves them and deletes them once they're off screen.
public class PipeScript : MonoBehaviour
{
    public GameObject pipePrefab;
    public float moveSpeed = 5f;
    public float spawnInterval = 5f;
    public float minY = -2.5f;
    public float maxY = 2.5f;
    public float spawnX = 12f;
    public float despawnX = -12f;
    public BirdScript bird; // optional: found automatically if left empty

    private readonly List<Transform> pipes = new List<Transform>();
    private float spawnTimer;

    // Lets other scripts (like ScoreScript) see the pipes that are on screen
    public IReadOnlyList<Transform> Pipes => pipes;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (bird == null) bird = FindAnyObjectByType<BirdScript>();
        SpawnPipe();
    }

    // Update is called once per frame
    void Update()
    {
        // Freeze the pipes once the bird is dead
        if (bird != null && !bird.isAlive) return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer -= spawnInterval;
            SpawnPipe();
        }

        // Loop backwards so pipes can be removed from the list while looping
        for (int i = pipes.Count - 1; i >= 0; i--)
        {
            Transform pipe = pipes[i];
            pipe.position += Vector3.left * moveSpeed * Time.deltaTime;

            if (pipe.position.x < despawnX)
            {
                pipes.RemoveAt(i);
                Destroy(pipe.gameObject);
            }
        }
    }

    void SpawnPipe()
    {
        Vector3 spawnPosition = new Vector3(spawnX, Random.Range(minY, maxY), 0f);
        GameObject newPipe = Instantiate(pipePrefab, spawnPosition, Quaternion.identity);
        pipes.Add(newPipe.transform);
    }
}
