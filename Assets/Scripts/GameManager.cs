using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("Value")]
    [SerializeField] public int minutes;
    [SerializeField] public float seconds;

    [Header("User Interface")]
    [SerializeField] private Text minutesText;
    [SerializeField] private Text secondsText;
    [SerializeField] private GameObject panelWin;
    [SerializeField] private GameObject panelLose;

    public static UnityEvent GameLose;
    public static UnityEvent GameWin;

    private Scene currentScene;

    //untuk cek kalo game dah selesai, update semua object berhenti. 
    //taruh di script update orang, furniture, monster juga
    public static bool isGameOver;

    void Start()
    {
        isGameOver = false;
        currentScene = SceneManager.GetActiveScene();
        GameWin = new UnityEvent();
        GameLose = new UnityEvent();
        GameWin.AddListener(ShowPanelWin);
        GameLose.AddListener(ShowPanelLose);
    }

    void Update()
    {
        if (!isGameOver)
        {
            CheckTimer();
            ShowTimer();
        }
    }

    private void CheckTimer()
    {
        if (seconds <= 0)
        {
            if (minutes <= 0)
            {
                PlayerPrefs.SetInt("levelReached", 2);
                ShowPanelWin();
            }
            else
            {
                minutes -= 1;
                seconds = 59;
            }
        }
        else
        {
            seconds -= Time.deltaTime;
        }
    }

    private void ShowTimer()
    {
        if(minutes < 10)
        {
            minutesText.text = "0" + minutes.ToString();
        }
        else
        {
            minutesText.text = minutes.ToString();
        }
        

        if(seconds < 9.5f)
        {
            secondsText.text = "0" + Mathf.RoundToInt(seconds).ToString();
        }
        else
        {
            secondsText.text = Mathf.RoundToInt(seconds).ToString();
        }
        
    }

    private void ShowPanelWin()
    {
        isGameOver = true;
        panelWin.SetActive(true);
    }

    private void ShowPanelLose()
    {
        isGameOver = true;
        panelLose.SetActive(true);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(currentScene.name);
    }

    public void NextLevel(int nextLevel)
    {
        SceneManager.LoadScene("Level" + nextLevel);
    }

    public void GoToLevelSelectorScene()
    {
        SceneManager.LoadScene("LevelSelector");
    }
}
