using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable] //class is serialized so we are able to convert it from cs object to json
public class GameData
{
    //class for the player stats that is being saved and loaded
    public long lastUpdated;
    public int Health;
    public int Potions;
    public Vector3 playerPosition;
    public int attackDamage;
    public int exp;
    public bool extraLife;
    public int level;

    //contructor that contains the default values when starting a new game (this is the goal of the constructor)
    public GameData(){
        this.level = 1;
        this.exp = 0;
        this.attackDamage = 10;
        this.Health = 100;
        this.Potions = 3;
        this.extraLife = true;
        playerPosition = new Vector3(-15.36f, -4.41f, -1.82634f);
    }

}
