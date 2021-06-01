using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penalty : MonoBehaviour
{
    private static Penalty instance = null;

    #region Singleton
    public static Penalty Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<Penalty>();
            }

            return instance;
        }
    }
    #endregion

    [SerializeField] private float penaltyTime = 2;

    [SerializeField] private GameObject penaltyModeText;

    public bool isPenalty
    {
        get; private set;
    }

    private void Update()
    {
        if (isPenalty)
        {
            PenaltyMode();
        }
    }

    private void PenaltyMode()
    {
        penaltyModeText.SetActive(true);
        penaltyTime -= Time.deltaTime;

        if(penaltyTime <= 0)
        {
            penaltyTime = 2;
            isPenalty = false;
            penaltyModeText.SetActive(false);
        }
    }

    public void IsPenalty(bool p)
    {
        isPenalty = p;
    }

}
