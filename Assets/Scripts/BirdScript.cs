using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BirdScript : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public float flapStrength = 10f;

    // How far below the bottom edge of the screen the bird can go before it dies
    public float offScreenMargin = 1f;

    // Other scripts (PipeScript, ScoreScript) check this to stop when the bird dies
    public bool isAlive = true;

    private Camera cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
        {
            // Press R to restart once dead
            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            return;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            myRigidbody.linearVelocity = Vector2.up * flapStrength;
        }

        // Die if the bird falls off the bottom of the screen
        if (cam != null)
        {
            float bottomOfScreen = cam.transform.position.y - cam.orthographicSize;
            if (transform.position.y < bottomOfScreen - offScreenMargin)
            {
                Die();
            }
        }
    }

    // Called by Unity when the bird's collider hits another collider (the pipes)
    void OnCollisionEnter2D(Collision2D collision)
    {
        Die();
    }

    void Die()
    {
        if (!isAlive) return;
        isAlive = false;
        Debug.Log("Game Over! Press R to restart.");
    }
}
