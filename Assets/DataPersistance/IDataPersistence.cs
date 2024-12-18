using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistence
{
    //interface for the load data and save data functions, this interface saves the data that is located in gamedata
    void LoadData(GameData data);
    void SaveData(GameData data);

}
