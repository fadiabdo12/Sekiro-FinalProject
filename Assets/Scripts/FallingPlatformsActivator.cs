using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformsActivator : MonoBehaviour
{
    private FallingPlatform [] fallingPlatforms;
    // Start is called before the first frame update
    void Start()
    {
        fallingPlatforms = FindObjectsOfType<FallingPlatform>();
    }

    void Update() {
        foreach (FallingPlatform platform in fallingPlatforms){
            if(!platform.isActiveAndEnabled){
                platform.gameObject.SetActive(true);
                platform.transform.position = platform.gameObject.GetComponent<FallingPlatform>().initialPositionObject;
                platform.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                platform.gameObject.GetComponent<FallingPlatform>().fallingObject = false;
            }
        }
    }

}
