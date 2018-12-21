using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    [SerializeField] GameObject restartBTN;

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            //QuitGame();
        }
    }

    public void LoadLevel(int levelToLoad)
    {
        SceneManager.LoadScene(levelToLoad);
    }
    public void Restart(int levelToLoad)
    {
        SceneManager.LoadScene(levelToLoad);
        restartBTN.SetActive(false);
    }

    public void QuitGame()
    {
        print("Quiting");
        Application.Quit();
    }
}
