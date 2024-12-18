using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPoint : MonoBehaviour
{
    private PlayerMovements _playerMovements;

    void Start()
    {
        _playerMovements = GameObject.FindWithTag("Player").GetComponent<PlayerMovements>();
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.tag != "Player")
            return;
    }
}
