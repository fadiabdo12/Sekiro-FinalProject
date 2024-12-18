using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FirstCutsceneTrigger : MonoBehaviour
{
    //this class is responsible for triggering the first cutscene in the first level of the game.
    
    [SerializeField] private PlayableDirector timelineDirector; // Assign the Timeline's PlayableDirector in the Inspector
    [SerializeField] private GameObject fadeCanvas;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to the player character (adjust the tag if needed)
        if (other.gameObject.CompareTag("Player"))
        {
            fadeCanvas.gameObject.SetActive(true);
            // // Play the Timeline when the player enters the trigger
            // if (timelineDirector != null)
            // {
            timelineDirector.Play();
        }
    }
}
