using UnityEngine;

public class triggerCanvas : MonoBehaviour
{
    //this class is for when the player enters the triggers for the tutorial canvases.
    private bool triggerable = true;
    private TutorialController _tutorialController;

    [Multiline]
    public string TextToShow;

    void Start()
    {
        _tutorialController = GameObject.FindWithTag("Tutorial").GetComponent<TutorialController>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(triggerable && other.gameObject.CompareTag("Player")){
            triggerable = false;            
            _tutorialController.Show(TextToShow);
        }
    }
}
