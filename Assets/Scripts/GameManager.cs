using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    //private Orang orang;

    void Start()
    {
        //orang = FindObjectOfType<Orang>();
    }

    void Update()
    {
        CheckTimer();
        ShowTimer();
    }

    private void CheckTimer()
    {
        //cek apakah npc di serang atau tidak
        //if (!orang.isAttacked)
        //{
        if (seconds <= 0)
        {
            if (minutes <= 0)
            {
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
        //}
        //else
        //{
            //gameplay berhenti
            //panel lose
            //ShowPanelLose();
        //}
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
        panelWin.SetActive(true);
    }

    private void ShowPanelLose()
    {
        panelLose.SetActive(true);
    }
}
