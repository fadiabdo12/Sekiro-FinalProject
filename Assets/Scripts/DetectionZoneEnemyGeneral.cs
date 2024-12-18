using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZoneEnemyGeneral : MonoBehaviour
{
    //this class is responsible for how the detection zone of enemy general behaves when
    //the player enters it
    private EnemyGeneral enemyGeneral;
    private bool inRange;
    private Animator enemyAnimator;

    private void Awake() {
        enemyGeneral = GetComponentInParent<EnemyGeneral>();
        enemyAnimator = GetComponentInParent<Animator>();
    }

    private void Update() {
        if(inRange && !enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("EnemyAttack")){
            enemyGeneral.Flip();
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        //function that is responisble for when the player enters the the trigger
        if(other.gameObject.CompareTag("Player")){
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        //function that is responisble for when the player exits the the trigger
        if(other.gameObject.CompareTag("Player")){
            inRange = false;
            gameObject.SetActive(false);
            enemyGeneral.TriggerArea.SetActive(true);
            enemyGeneral.inRange = false;
            enemyGeneral.SelectTarget();
        }
    }
}
