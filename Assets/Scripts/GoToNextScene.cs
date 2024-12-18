using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToNextScene : MonoBehaviour
{
    //this function is used for when the game loads the next level after a certain cutscene in the event,
    //usually when the cutscene is on the end of the level. also this function is mainly used as an event in
    //the unity engine.
    [SerializeField]private GameObject loadingScreen;

    private void Update(){
        if (Input.GetKeyDown(KeyCode.Q)){
            NextScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void NextScene(int sceneIndex){
        //this function is responsible for loading a level asynchronously for the loading screen
        // DataPersistenceManager.instance.GameData.level +=1;
        // gameManager.LoadLevel(sceneIndex);
        Debug.Log("first save");
        DataPersistenceManager.instance.SaveGame();
        LoadLevel(sceneIndex);
        Debug.Log("second save");

        
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
