using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitBoxScript : MonoBehaviour
{
    //this class is responsible for the hit box of the enemy.
    private PlayerBars playerBars;
    private AudioManager audioManager;
    private PlayerMovements playerMovements;

    private void Awake() {
        playerBars = GetComponent<PlayerBars>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        playerMovements = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovements>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // if the hit box of the enemy did hit the player and he wasn't blocking.
        if(!Input.GetKey(KeyCode.LeftAlt) && other.CompareTag("Player") || (playerMovements.blockCooldownTrigger == true && Input.GetKey(KeyCode.LeftAlt))){
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            playerStats.TakeDamage(20);
            audioManager.PlaySFX(audioManager.getHitSound);
        }
        else
        {
            if(Input.GetKey(KeyCode.LeftAlt) && other.gameObject.CompareTag("Player")){
                //if the hitbox did hit the player and he was blocking.
                PlayerStats playerStats = other.GetComponent<PlayerStats>();
                //PlayerBars playerBars = GetComponent<PlayerBars>();
                audioManager.PlaySFX(audioManager.blockSound);
                playerStats.currentPosture += 20;

            }
        }
    }
}
