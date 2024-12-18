using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float fallDelay = 1.5f;
    [SerializeField] private float destroyDelay = 5f;
    private AudioManager audioManager;

    private bool falling = false;

    [SerializeField] private Rigidbody2D rb;

    private Vector3 initialPosition;

    public Vector3 initialPositionObject{
        get { return initialPosition; }
        set { initialPosition = value; }
    }

    public Rigidbody2D rbObject{
        get { return rbObject; }
        set { rbObject = value; }
    }

    public bool fallingObject{
        get { return falling; }
        set { falling = value; }
    }

    private void Start()
    {
    // Save the initial position of the GameObject
    initialPosition = transform.position;
    audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Avoid calling the coroutine multiple times if it's already been called (falling)
        if (falling)
            return; 

        // If the player landed on the platform, start falling
        if (collision.transform.tag == "Player")
        {
            audioManager.PlaySFX(audioManager.RocksFallingSound);
            StartCoroutine(StartFall());
        }
    }

    private IEnumerator StartFall()
    {
        falling = true;
        // Wait for a few seconds before dropping
        yield return new WaitForSeconds(fallDelay);

        // Enable rigidbody and destroy after a few seconds
        rb.bodyType = RigidbodyType2D.Dynamic; 

        yield return new WaitForSeconds(destroyDelay);
        gameObject.SetActive(false);
    }
    

}
