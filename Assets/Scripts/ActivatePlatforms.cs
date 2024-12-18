using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActivatePlatforms : MonoBehaviour
{
    //Script that is responsible for activating the trap platforms
    [SerializeField] GameObject platformOne;
    [SerializeField] GameObject platformTwo;

    private void Update() {
        //if player interacted with the gameobject then the platforms gets activated
        if(this.GetComponent<Canvas>().enabled){
            platformOne.SetActive(true);
            platformTwo.SetActive(true);
        }
    }

}
