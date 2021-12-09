using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        //if ESCAPE is pressed, change scene to MainMenu
        if(Input.GetKeyDown(KeyCode.Escape)) {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }
}
