using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAreaCheck : MonoBehaviour
{
    //this class for when the player enters the trigger area of the enemy.
    private EnemySoldier enemySoldier;
    private Animator enemyAnimator;

    private void Awake() {
        enemySoldier = GetComponentInParent<EnemySoldier>();
        enemyAnimator = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        //when the player enters the trigger area of the enemy
        if(other.gameObject.CompareTag("Player")){
            gameObject.SetActive(false);
            enemySoldier.target = other.transform;
            enemySoldier.inRange = true;
            enemySoldier.DetectionZone.SetActive(true);

        }
    }
}
