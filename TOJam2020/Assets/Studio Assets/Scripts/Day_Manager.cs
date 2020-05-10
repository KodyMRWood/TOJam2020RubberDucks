﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class Day_Manager : MonoBehaviour
{
    public TextMeshProUGUI clock;
    public float dayLengthIRL = 10.0f;
    private float dayLengthIG = 0.0f;

    public float dayStartTime = 9.0f;
    public float dayEndTime = 17.0f;

    private float timeElapsed = 0.0f;
    private float timeElapsedLERP = 0.0f;
    private float timeElapsedIGLERP = 0.0f;

    private int hours = 0;
    private int minutes = 0;

    private string timeString = " ";

    public int dayCounter = 0;

    public bool playing = true;

    public GameObject cashCalculator;
    public GameObject singletonEventSystem;


    // Start is called before the first frame update
    void Start()
    {
       
        dayLengthIG = dayEndTime - dayStartTime;
        timeElapsed = 0.0f;


        GameObject.FindObjectOfType<Day_Manager>();
        singletonEventSystem = GameObject.Find("EventSystem");
       if(singletonEventSystem != null )
       {
            Destroy(singletonEventSystem);
       }
       else
        {
            singletonEventSystem.name = "EventSystemForever";
        }

        clock = null ?? GameObject.Find("Clock").GetComponent<TextMeshProUGUI>();
        cashCalculator = null ?? GameObject.Find("EventSystem");


        

    }

    // Update is called once per frame
    void Update()
    {
        //If we want to keep this function this class
            timeElapsed = timeElapsed + Time.deltaTime;
            //Day end protocol
            if (timeElapsed >= dayLengthIRL)
            {
            //Call CalculateEndOfDayMoney
            this.GetComponent<CashCalculation_Script>().CalculateCashForDay();

                //Incerment Day counter
                dayCounter++;

                //Reset
                timeElapsed = 0.0f;


            if (dayCounter == 7)
            {
                //End week and game
                DontDestroyOnLoad(this.gameObject);
                SceneManager.LoadScene("EndOfWeek");
            }
            else if (dayCounter == 2)
            {
                DontDestroyOnLoad(this.gameObject);
                SceneManager.LoadScene("Kody");
            }
            else
            {

                //End day
                DontDestroyOnLoad(this.gameObject);
                SceneManager.LoadScene("EndOfDay");
            }

            }

        if (clock)
        {
            //Fine Time elapsed / time remaining
            timeElapsedLERP = timeElapsed / dayLengthIRL; // This is the percent of the day that has gone by

            dayLengthIG = (dayEndTime * 60.0f) - (dayStartTime * 60.0f);

            Debug.Log("LErp: " + timeElapsedLERP);

            //timeElapsedIGLERP = dayLengthIG * timeElapsedLERP;
            timeElapsedIGLERP = Mathf.Lerp((dayStartTime * 60.0f), (dayEndTime * 60.0f), timeElapsedLERP);

            Debug.Log("DayLErp: " + timeElapsedIGLERP);

            hours = (int)timeElapsedIGLERP / 60;
            minutes = (int)timeElapsedIGLERP % 60;

            timeString = hours.ToString("00") + ":";

            timeString += minutes.ToString("00");

            clock.GetComponent<TextMeshProUGUI>().text = timeString;
        }
    }
}
