using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverHandler : MonoBehaviour
{

    public GameObject MainMenu;
    public GameObject Credits;

    public GameObject gameOverReasonTextBox;
    public GameObject scoreTextBox;


    // Start is called before the first frame update
    void Start()
    {
        ActivateMenu("main");
    }

    public void GoToCredits() {
        ActivateMenu("credits");
    }

    public void GoToGameOver() {
        ActivateMenu("main");
    }

    public void GoToMainMenu() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void ActivateMenu(string menu) {
        switch (menu)
        {
            case "main":
                MainMenu.SetActive(true);
                Credits.SetActive(false);
                
                gameOverReasonTextBox.GetComponent<UnityEngine.UI.Text>().text = PlayerPrefs.GetString("gameOverReason", "Game over :(");
                string scoreString = "Score: " + PlayerPrefs.GetInt("score", 0).ToString();
                scoreTextBox.GetComponent<UnityEngine.UI.Text>().text = scoreString;
                
                break;

            case "credits":
                MainMenu.SetActive(false);
                Credits.SetActive(true);
                break;

            default:
                MainMenu.SetActive(true);
                Credits.SetActive(false);
                break;
        }
    }

}
