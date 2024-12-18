using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FinalCutsceneTrigger : MonoBehaviour
{
    [SerializeField] private PlayableDirector timelineDirector;
    [SerializeField] private GameObject enemyBoss;
    private CapsuleCollider2D BossCapsuleCollider;

    void Start() {
        BossCapsuleCollider = enemyBoss.GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        if(!BossCapsuleCollider.enabled){
            timelineDirector.Play();
            this.gameObject.SetActive(false);
        }
    }
}
