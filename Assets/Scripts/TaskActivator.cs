using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskActivator : MonoBehaviour
{
    [SerializeField] private Canvas TaskCanvas;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){
                TaskCanvas.enabled = true;
                Time.timeScale = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        Destroy(this);
    }
}
