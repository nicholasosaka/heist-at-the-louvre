using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    Alert alert;
    ScoreHandler scoreHandler;
    
    void Start() {
        alert = new Alert();
        scoreHandler = new ScoreHandler();
    }

    // Update is called once per frame
    void Update()
    {
        //if ESCAPE is pressed, change scene to MainMenu
        if(Input.GetKeyDown(KeyCode.Escape)) {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }

        if(alert.GetAlertLevel() == alert.Max()) {
            PlayerPrefs.SetString("gameOverReason", "The guards were alerted! You lost.");
            PlayerPrefs.SetInt("score", scoreHandler.GetScore());
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
        }

        if(scoreHandler.GetScore() == scoreHandler.GetMax()) {
            PlayerPrefs.SetString("gameOverReason", "The Louvre was robbed! You won.");
            PlayerPrefs.SetInt("score", scoreHandler.GetScore());
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
        }
    }
}
