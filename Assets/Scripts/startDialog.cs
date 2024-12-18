using System.Collections;
using System.Collections.Generic;
using cherrydev;
using Unity.VisualScripting;
using UnityEngine;

public class startDialog : MonoBehaviour
{
    //this class is responsible for triggering the dialog of the first scene
    [SerializeField] private DialogBehaviour dialogBehaviour;
    [SerializeField] private DialogNodeGraph dialogGraph;
    [SerializeField] private GameObject dialogBox;

    PlayerMovements playerMovements;

    private void Start() {
        playerMovements = FindObjectOfType<PlayerMovements>();
    }

    private void StartTalking(){
        dialogBox.gameObject.SetActive(true);
        dialogBehaviour.StartDialog(dialogGraph);
        
    }

}
