using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject creditPanel;

    public void Play(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void Setting()
    {
        settingPanel.SetActive(true);
    }

    public void Tutorial()
    {
        tutorialPanel.SetActive(true);
    }

    public void Credit()
    {
        creditPanel.SetActive(true);
    }

    public void Back()
    {
        if (settingPanel.activeSelf == true)
        {
            settingPanel.SetActive(false);
        }

        if(tutorialPanel.activeSelf == true)
        {
            tutorialPanel.SetActive(false);
        }

        if(creditPanel.activeSelf == true)
        {
            creditPanel.SetActive(false);
        }
    }
}
