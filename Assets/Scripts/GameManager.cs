using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //this class is responsible for controling how the game behaves when pressing certain options on the main menu
    [SerializeField]private Canvas playerDeath;
    private TextMeshProUGUI textAnimation;
    [SerializeField]private GameObject loadingScreen;

    [Header("Levels to Load")]
    [SerializeField] private Button loadGame;
    [SerializeField] private GameObject continueButton;

    // Start is called before the first frame update
    void Start()
    {
        textAnimation = FindObjectOfType<TextMeshProUGUI>();
        playerDeath.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //this if statment is basically to check if the game has saved data, if it does then the continue button will be visible for the player and the load button will be available to interact with. if not then the opposite
        if(DataPersistenceManager.instance.HasGameData()){
            continueButton.SetActive(true);
            loadGame.interactable = true;
        }
        else{
            continueButton.SetActive(false);
            loadGame.interactable = false;
        }
    }

    public void ContinueGame(){
        //this function is for when the player presses the continue game button

        //save the game anytime before loading a new scene
        DataPersistenceManager.instance.SaveGame();

        LoadLevel(DataPersistenceManager.instance.GameData.level);
    }

    public void LoadLevel(int sceneIndex){
        //this function is responsible for loading a level asynchronously for the loading screen
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex){
        //this function is forthe loading screen to load asynchronously
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);

        while(!operation.isDone){
            float progress = Mathf.Clamp01(operation.progress / .9f);
            yield return null;
        }
    }
}
