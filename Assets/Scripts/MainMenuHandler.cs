using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{

    public GameObject MainMenu;
    public GameObject Credits;
    public GameObject Instructions;

    // Start is called before the first frame update
    void Start()
    {
        ActivateMenu("main");
    }

    public void GoToCredits() {
        ActivateMenu("credits");
    }

        public void GoToInstructions() {
        ActivateMenu("instructions");
    }

    public void GoToGame() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    public void GoToMain() {
        ActivateMenu("main");
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void ActivateMenu(string menu) {
        switch (menu)
        {
            case "main":
                MainMenu.SetActive(true);
                Credits.SetActive(false);
                Instructions.SetActive(false);
                break;

            case "credits":
                MainMenu.SetActive(false);
                Credits.SetActive(true);
                Instructions.SetActive(false);
                break;

            case "instructions":
                MainMenu.SetActive(false);
                Credits.SetActive(false);
                Instructions.SetActive(true);
                break;

            default:
                MainMenu.SetActive(true);
                Credits.SetActive(false);
                Instructions.SetActive(false);
                break;
        }
    }

}
