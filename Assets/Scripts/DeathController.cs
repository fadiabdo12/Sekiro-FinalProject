using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathController : MonoBehaviour
{
    //this class is responsible for controlling how the death canvas works, meaning when the player dies.
    PlayerStats playerStats;
    PlayerMovements playerMovements;
    // Start is called before the first frame update
    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        playerMovements = FindObjectOfType<PlayerMovements>();
    }

    public void ReserructPlayer(){
        //function for what happens when the player presses the reserruct option.
        playerStats.currentHealth = 50;
        playerStats.isAlive = true;
        playerStats.playerExtraLife = false;
        playerMovements.blockCooldownTrigger = false;
        playerMovements.gameObject.SetActive(false);
        playerMovements.gameObject.SetActive(true);
        playerMovements.DeathCanvasObject.gameObject.SetActive(false);
    }

    public void RestartCheckPoint(){
        //function for when the player presses the restart from checkpoint option
        playerStats.currentHealth = 100;
        playerStats.isAlive = true;
        playerStats.currentPotions = 3;
        playerMovements.gameObject.SetActive(false);
        DataPersistenceManager.instance.LoadGame();
        playerMovements.gameObject.SetActive(true);
        playerMovements.DeathCanvasObject.gameObject.SetActive(false);
    }
}
