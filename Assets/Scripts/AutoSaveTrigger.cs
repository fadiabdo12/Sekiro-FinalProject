using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSaveTrigger : MonoBehaviour
{
    //this class is responsible for triggering the auto save function in the game (checkpoint)

    [SerializeField] private GameObject AutoSaveText;
    [SerializeField] private GameObject AutoSaveAnim;
    [SerializeField] private GameObject ExpBar;
    PlayerMovements playerMovements;

    private void Start() {
        playerMovements = FindObjectOfType<PlayerMovements>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        //function that is responisble for when the player enters the the trigger
        if(other.gameObject.CompareTag("Player")){
            ExpBar.SetActive(false);
            AutoSaveText.gameObject.SetActive(true);
            AutoSaveAnim.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        //function that is responisble for when the player exits the the trigger
        if(other.gameObject.CompareTag("Player")){
            DataPersistenceManager.instance.SaveGame();
            gameObject.SetActive(false);
            ExpBar.SetActive(true);
            AutoSaveText.gameObject.SetActive(false);
            AutoSaveAnim.gameObject.SetActive(false);
        }
    }
}
