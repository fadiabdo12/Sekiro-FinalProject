using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
  //this class is responsible for managing saving data in the game and the logic behind saving and loading the game
  [Header("Debugging")]
  [SerializeField] private bool disableDataPersistence = false;
  [SerializeField] private bool initializeDataIfNull = false;

  [Header("File Storage Config")]
  [SerializeField] private string fileName;
  [SerializeField] private bool useEncryption;
  private GameData gameData;
  private List<IDataPersistence> dataPersistencesObjects;
  private FileDataHandler dataHandler;
  private string selectedProfileId = "";

  public static DataPersistenceManager instance {get; private set;}
  
  public GameData GameData
  {
    get { return gameData; }
    set { gameData = value; }
  }

  private void Awake() {
    if(instance != null){
      Debug.LogError("Found more than one Data Persistence Manager in the scene. Destroying the newest one");
      Destroy(this.gameObject);
      return;
    }
    instance = this;
    DontDestroyOnLoad(this.gameObject);

    if(disableDataPersistence)
    {
      Debug.LogWarning("data persistence is currently disabled!");
    }

    this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);

    this.selectedProfileId = dataHandler.GetMostRecentlyUpdatedProfileId();
  }

  private void OnEnable() {
    SceneManager.sceneLoaded += onSceneLoaded;
  }

  private void OnDisable() {
    SceneManager.sceneLoaded -= onSceneLoaded;
  }

  public void onSceneLoaded(Scene scene, LoadSceneMode mode){
    print("on scene loaded!");
      this.dataPersistencesObjects = FindAllDataPersistenceObjects();
      LoadGame();
      SaveGame();
  }

  private List<IDataPersistence> FindAllDataPersistenceObjects(){
    //FindObjectsOfType takes an optional boolean to include inactive objects
    //store a refrence to each IdataPersistence script in a list, in our case PlayerStats and PlayerMovements
    IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();

    return new List<IDataPersistence>(dataPersistenceObjects);
  }

  public void ChangeSelectedProfileId(string newProfileId){
    //update the profile to use for saving and loading
    this.selectedProfileId = newProfileId;
    
    //load the game, which will use that profile, updating our game accordingly
    LoadGame();
  }

  //NEW GAME!!!!!
  public void NewGame(){
    //when new game is selected we create new gamedata object 
    this.gameData = new GameData();
  }

  //LOAD GAME!!!!
  public void LoadGame(){
    //return right away if data persistence is disabled
    if(disableDataPersistence)
    {
      return;
    }
    //load any saved data from a file using data handler
    this.gameData = dataHandler.Load(selectedProfileId);
    //start a new game if the data is null and we're configured to initialize data for debugging purposes(DEBUG)
    if(this.gameData == null && initializeDataIfNull)
    {
      NewGame();
    }

    //if no data can be loaded(not found). return...
    if(this.gameData == null){
      Debug.Log("No data was found. a new game needs to be started before data can be loaded ");
      return;
    }
    
    print("pre loadData!!!!");
    foreach(IDataPersistence dataPersistenceObj in dataPersistencesObjects)
    {
      dataPersistenceObj.LoadData(gameData);// LOADING THE DATA TO THE GAMEOBJECT(PLAYERMOVEMENT,PLAYERSTATS)
    }
  }

  //SAVE GAME!!!
  public void SaveGame(){
    //return right away if data persistence is disabled
    if(disableDataPersistence)
    {
      return;
    }

    //if we don't have any data to save, log a warning here
    if(this.gameData == null){
      Debug.LogWarning("no data was found. a new game needs to be started before data can be saved");
    }

    //pass the data to other scripts so they can update it
    foreach(IDataPersistence dataPersistenceObj in dataPersistencesObjects)
    {
      dataPersistenceObj.SaveData(gameData);// SAVING THE DATA FROM THE GAMEOBJECT(PLAYERMOVEMENT,PLAYERSTATS)
    }

    //timestamp the data so we know when it was last saved
    gameData.lastUpdated = System.DateTime.Now.ToBinary();
    
    //save that data to a file using the data handler
    dataHandler.Save(gameData, selectedProfileId);
  }


  public bool HasGameData()
  {
    return gameData != null;
  }

  public Dictionary<string, GameData> GetAllProfilesGameData(){
    return dataHandler.LoadAllPorfiles();
  }

}
