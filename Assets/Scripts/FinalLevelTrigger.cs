using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalLevelTrigger : MonoBehaviour
{
    [SerializeField]private GameObject loadingScreen;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){
            DataPersistenceManager.instance.SaveGame();
            NextScene(4);
            DataPersistenceManager.instance.SaveGame();
        }
    }


    public void NextScene(int sceneIndex){
        Debug.Log("nextlevelFunctinoActivated");
        //this function is responsible for loading a level asynchronously for the loading screen
        // DataPersistenceManager.instance.GameData.level +=1;
        // gameManager.LoadLevel(sceneIndex);
        DataPersistenceManager.instance.SaveGame();
        LoadLevel(sceneIndex);
        DataPersistenceManager.instance.SaveGame();
        
    }

    public void LoadLevel(int sceneIndex){
        Debug.Log("loadLevelFuncitionActivated");
        //this function is responsible for loading a level asynchronously for the loading screen
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex){
        //this function is forthe loading screen to load asynchronously
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        Debug.Log("loadingAsync");

        loadingScreen.SetActive(true);

        while(!operation.isDone){
            float progress = Mathf.Clamp01(operation.progress / .9f);
            yield return null;
        }
    }
}
