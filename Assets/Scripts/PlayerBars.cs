using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerBars : MonoBehaviour
{
    //this class is responsible for controlling the UI HP bar and Posture bar
    [SerializeField] Slider hpSlider;
    [SerializeField] Slider postureSlider;
    [SerializeField] TextMeshProUGUI potions;

    public void setMaxHealth(int hp){
        //set function for max health
        hpSlider.maxValue = hp;
        hpSlider.value = hp;
    }

    public void setHealth(int hp){
        //set function for the health
        hpSlider.value = hp;
    }
    public void setMaxPosture(int posture){
        //set function for the max posture
        postureSlider.maxValue = posture;
        postureSlider.value = posture;
    }

    public void setPosture(int posture){
        //set function for the posture
        postureSlider.value = posture;
    }

    
}
