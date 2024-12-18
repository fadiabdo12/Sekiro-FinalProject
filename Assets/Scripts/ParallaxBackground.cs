using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    //this class is responsible for the parallax effect of the background
    [SerializeField] Vector2 parallaxEffectMultiplier;

    // LateUpdate is called after Update. It's often used for camera-related calculations.
    void LateUpdate()
    {
         // Get the current position of the main camera.
        var cameraPosition = Camera.main.transform.position;
        // Calculate the new position for the background based on the camera's position
        // and the parallax effect multipliers.
        // The background moves horizontally (X axis) with a factor of parallaxEffectMultiplier.x,
        // and stays vertically (Y axis) in line with the camera's Y position.
        transform.position = new Vector2(cameraPosition.x * parallaxEffectMultiplier.x, cameraPosition.y);
    }
}
