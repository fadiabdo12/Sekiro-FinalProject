using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSlotsMenu : MonoBehaviour
{
    private SaveSlot [] saveSlots;
    private GameObject chooseSave;
    [SerializeField]private GameManager gameManager;

    private bool isLoadingGame = false;

    private void Awake() 
    {
        saveSlots = this.GetComponentsInChildren<SaveSlot>();
    }

    public void onSaveSlotClicked(SaveSlot saveSlot){

        //update the selected profile id to be used for data persistence
        DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());

        if(!isLoadingGame){
        //create new game which will initialize our data to a clean slate
        DataPersistenceManager.instance.NewGame();
        }
        
        //save the game anytime before loading a new scene
        DataPersistenceManager.instance.SaveGame();

        gameManager.LoadLevel(DataPersistenceManager.instance.GameData.level);
    }

    public void AcitvateMenu(bool isLoadingGame){
        this.isLoadingGame = isLoadingGame;

        //load all of the profiles that exist
        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.instance.GetAllProfilesGameData();

        //loop through each save slot in the UI and set the content appropriately
        foreach(SaveSlot saveSlot in saveSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);
            if(profileData == null && isLoadingGame){
                saveSlot.gameObject.SetActive(false);
            }
            else{
                saveSlot.gameObject.SetActive(true);
            }

        }
    }

}
