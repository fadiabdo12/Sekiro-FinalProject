using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAreaEnemyGeneral : MonoBehaviour
{
    //this class for when the player enters the trigger area of the enemy.
    private EnemyGeneral enemyGeneral;
    // Start is called before the first frame update
    void Start()
    {
        enemyGeneral = GetComponentInParent<EnemyGeneral>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        //when the player enters the trigger area of the enemy
        if(other.gameObject.CompareTag("Player")){
            gameObject.SetActive(false);
            enemyGeneral.target = other.transform;
            enemyGeneral.inRange = true;
            enemyGeneral.DetectionZone.SetActive(true);
        }
    }
}
