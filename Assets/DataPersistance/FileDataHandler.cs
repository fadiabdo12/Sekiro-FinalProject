using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    //this class is responsible for handling the saved file. writing the save file, etc...
    private string dataDirPath = "";
    private string dataFileName = "";
    private bool useEncryption = false;// Optionally to encrypt
    private readonly string encryptionCodeWord = "panda";// encryptoin codeword

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption){
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    public GameData Load(string profileId){

        //base case - if the profileid is null, return right away
        if(profileId == null){
            return null;
        }

        //use path.combine to account for different OS's having different path seperators
        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);

        GameData loadedData = null;
        if(File.Exists(fullPath))
        {
            try
            {
                //load the serialized data from the file
                string dataToLoad = ""; //variable for the data
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))//creating a file stream to
                {                                                                  //the saved file and then a
                    using(StreamReader reader = new StreamReader(stream))          //reader to read from the
                    {                                                              //stream that we created
                        dataToLoad = reader.ReadToEnd();                           //and all goes to datatoLoad
                    }
                }
            //optionally dycrypt the data
            if(useEncryption)
            {
                dataToLoad = EncryptDecrypt(dataToLoad);
            }
            
            // deserialize the data from Json back into the C# object
            loadedData = JsonUtility.FromJson<GameData>(dataToLoad);

            }
            catch(Exception e){
                Debug.LogError("Error occured when trying to load data to file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;// return the loaded data
    }

    //SAVE TO FILE, this function is responsible for creating a directory, path, file if not created and saving the data to a file.
    public void Save(GameData data, string profileId){
        //base case - if the profileid is null, return right away
        if(profileId == null){
            return;
        }

        //use path.combine to account for different OS's having different path seperators
        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        try
        {
            //create the directory the file will be written to if it doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            //serialize the c# game data object into json 
            string dataToStore = JsonUtility.ToJson(data, true);

            //optionally encrypt the data
            if(useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            //write the serialized data to the file
            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    public Dictionary<string, GameData> LoadAllPorfiles(){
        Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();

        //loop over all directory names in the data directory path
        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
        //EnumarateDirectories gives us an info of the dicrectories that is in that path, in this case we need the name...
        foreach(DirectoryInfo dirInfo in dirInfos){
            string profileId = dirInfo.Name;

            //if data file doesn't exist, then the folder is not a profile and should be skipped
            string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
            if(!File.Exists(fullPath)){
                Debug.LogWarning("Skipping directory when loading all profiles because it does not contain data: " + profileId);
                continue;
            }

            //if it does exist, load the game data for this profile and put it in the dictionary
            GameData profileData = Load(profileId);

            //bonus check if data is not null because that means something is wrong
            if(profileData != null){
                profileDictionary.Add(profileId, profileData);// adding the profile and it's data to the dic
            }
            else{
                Debug.LogError("Tried to load profile but something went wrong. ProfileId: " + profileId);
            }
        }

        return profileDictionary;
    }

    public string GetMostRecentlyUpdatedProfileId(){
        string mostRecentProfileId = null;

        Dictionary<string, GameData> profilesGameData = LoadAllPorfiles();
        foreach(KeyValuePair<string, GameData> pair in profilesGameData){
            string profileId = pair.Key;
            GameData gameData = pair.Value;

            //skip this entry if the gamedata is null, although from the func LoadAllProfiles there shouldn't be
            if(gameData == null){
                continue;
            }

            //if this is the first data we've come across that exists, it's the most recent so far
            if(mostRecentProfileId == null)
            {
                mostRecentProfileId = profileId;
            }
            //otherwise. compare to see which date is the most recent
            else
            { //basically like returning the max number in an array.. lol...
                //we use from binary because they are saved as a binary in json file
                DateTime mostRecentDateTime = DateTime.FromBinary(profilesGameData[mostRecentProfileId].lastUpdated);
                DateTime newDateTime = DateTime.FromBinary(gameData.lastUpdated);
                //the greatest DateTime value is the most recent
                if(newDateTime > mostRecentDateTime)
                {
                    mostRecentProfileId = profileId;
                }
            }
        }
        return mostRecentProfileId;
    }

    //this is XOR encryption, which takes one character of the data and does an xor OP on another character
    private string EncryptDecrypt(string data){
        string modifiedData = "";
        for(int i = 0; i < data.Length; i++) 
        {
            modifiedData += (char) (data[i] ^ encryptionCodeWord [i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }

}
