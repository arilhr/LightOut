using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject creditPanel;

    [SerializeField] private AudioSource bgm;
    [SerializeField] private AudioSource buttonClick;
    [SerializeField] private Slider slider;

    private void Start()
    {
        bgm = GetComponent<AudioSource>();
        slider.value = 0.5f;
        PlayerPrefs.SetFloat("Volume", slider.value);
        bgm.Play();
    }

    private void Update()
    {
        bgm.volume = slider.value;
        buttonClick.volume = slider.value;
        PlayerPrefs.SetFloat("Volume", slider.value);
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Quit();
            }
        }
    }

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

    public void PlaySFX()
    {
        buttonClick.Play();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
