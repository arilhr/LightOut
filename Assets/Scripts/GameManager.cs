using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    #region Singleton
    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }

            return instance;
        }
    }
    #endregion

    [Header("Value")]
    [SerializeField] public int minutes;
    [SerializeField] public float seconds;
    public bool isGameOver;

    [Header("User Interface")]
    [SerializeField] private Text minutesText;
    [SerializeField] private Text secondsText;
    [SerializeField] private GameObject panelWin;
    [SerializeField] private GameObject panelLose;
    [SerializeField] private GameObject panelPause;

    [Header("Sounds")]
    [SerializeField] private AudioSource bgm;
    [SerializeField] private AudioSource buttonClick;

    public UnityEvent GameLose;
    public UnityEvent GameWin;

    private Scene currentScene;
    private bool isPaused = false;

    public bool IsPaused
    {
        get { return isPaused; }
    }

    void Start()
    {
        isGameOver = false;
        currentScene = SceneManager.GetActiveScene();
        GameWin = new UnityEvent();
        GameLose = new UnityEvent();
        GameWin.AddListener(ShowPanelWin);
        GameLose.AddListener(ShowPanelLose);
        bgm = GetComponent<AudioSource>();
        bgm.volume = PlayerPrefs.GetFloat("Volume");
        buttonClick.volume = PlayerPrefs.GetFloat("Volume");
        PlayBGM();
    }

    void Update()
    {
        if (!isGameOver && !isPaused)
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

    public void Pause()
    {
        panelPause.SetActive(true);
        isPaused = true;
    }

    public void Resume()
    {
        isPaused = false;
        panelPause.SetActive(false);
    }

    private void PlayBGM()
    {
        bgm.Play();
    }

    public void PlaySFX()
    {
        buttonClick.Play();
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
    }
}
