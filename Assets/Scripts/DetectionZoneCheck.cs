using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZoneCheck : MonoBehaviour
{
    //this class is responsible for how the detection zone of enemy soldier behaves when
    //the player enters it
    private EnemySoldier enemyParent;
    private bool inRange;
    private Animator enemyAnimator;

    private void Awake() {
        enemyParent = GetComponentInParent<EnemySoldier>();
        enemyAnimator = GetComponentInParent<Animator>();
    }

    private void Update() {
        if(inRange && !enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("EnemyAttack")){
            enemyParent.Flip();
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        ////function that is responisble for when the player enters the the trigger
        if(other.gameObject.CompareTag("Player")){
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        //function that is responisble for when the player exits the the trigger
        if(other.gameObject.CompareTag("Player")){
            inRange = false;
            gameObject.SetActive(false);
            enemyParent.TriggerArea.SetActive(true);
            enemyParent.inRange = false;
            enemyParent.SelectTarget();
        }
    }
}
