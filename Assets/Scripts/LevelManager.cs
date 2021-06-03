using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Button[] levelButtons;

    private void Start()
    {
        ConfigureButtonLevel();
    }

    private void ConfigureButtonLevel()
    {
        int levelReached = PlayerPrefs.GetInt("levelReached", 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 > levelReached)
            {
                levelButtons[i].interactable = false;
            }
        }
    }

    public void GoToScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    /*public void ResetLevel()
    {
        PlayerPrefs.DeleteKey("levelReached");
        ConfigureButtonLevel();
    }*/
}
