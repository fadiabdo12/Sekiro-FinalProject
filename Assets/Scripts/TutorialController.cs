using UnityEngine;
using TMPro;

public class TutorialController : MonoBehaviour
{
    //this function is to control the tutorial canvas. meaning changing the text.
    private bool _shown;
    
    public Canvas TutorialCanvas;
    public TextMeshProUGUI Text;
    public GameObject continueText;
    public AudioSource AudioSource;
    private Animator textAnimator;

    private void Start() {
        TutorialCanvas.enabled = false;
        textAnimator = GetComponent<Animator>();
        continueText.SetActive(false);
    }

    public void Show(string text){
        _shown = true;
        TutorialCanvas.enabled = true;
        continueText.SetActive(true);
        Text.text = text;
        AudioSource.Play();
        Time.timeScale = 0f;
    }

    private void Update(){
        //for the press spacebar to continue button
        if (_shown && Input.GetKey(KeyCode.Space)){
            Hide();
        }
    }

    private void Hide(){
        //hide the trigger after the player goes in it and exits it
        _shown = false;
        TutorialCanvas.enabled = false;
        Time.timeScale = 1f;
    }
}
