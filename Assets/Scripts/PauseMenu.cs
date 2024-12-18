using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    //this class is responsible for controlling the pause menu screen
    public bool _gameIsPaused;
    [SerializeField] private Canvas pauseMenu;
    private PlayerMovements playerMovements;

    private void Start() {
        pauseMenu.enabled = false;
        playerMovements = FindObjectOfType<PlayerMovements>();
    }

    public void Resume(){
        //what happens when the game is resumed
        pauseMenu.enabled = false;
        Time.timeScale = 1f;
        _gameIsPaused = false;
    }

    public void Pause(){
        //what happens when the game is paused
        pauseMenu.enabled = true;
        Time.timeScale = 0f;
        _gameIsPaused = true;
    }

    public bool GameIsPaused(){
        //to check if the game is paused
        return _gameIsPaused;
    }

    public void GoMainMenu(){
        //if go back to main menu button is pressed
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void QuitGame(){
        //if the player chooses the quit game button
        Application.Quit();
    }
}
