﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CallerLog_Script : MonoBehaviour
{
    //--- Global Variable ---//
    //UI Variables
    public TextMeshProUGUI callers;
    public TextMeshProUGUI callDuration;
    public Image timerBar;
    public Image timerClock;
    public TextMeshProUGUI[] bindingLetters;
    public GameObject[] blockingImages;

    //Caller Variables

    private Call_Group refGroup;
    //Call_Group groupCall;
    private int numCallers = 0;
    private float waitTimeMax = 0.0f;
    private float waitTimeRemaining = 0.0f;
    private float callTimeMax = 0.0f;
    private float callTimeRemaining = 0.0f;

    private float waitTimePercent = 0.0f;
    private float callTimePercent = 0.0f;


    // Start is called before the first frame update
    void Start()
    {

        numCallers = refGroup.GetNumParticipants();
        waitTimeMax = refGroup.GetWaitTimeMax();
        waitTimeRemaining =refGroup.GetWaitTimeRemaining();
        callTimeMax = refGroup.GetCallTimeMax();
        callTimeRemaining = refGroup.GetCallTimeRemaining();

        callers.GetComponent<TextMeshProUGUI>().text= numCallers.ToString();
        callDuration.GetComponent<TextMeshProUGUI>().text = callTimeMax.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        //timerBar.GetComponent<Image>().fillAmount = waitTimeRemaining / waitTimeMax;

        //timerClock.GetComponent<Image>().fillAmount = callTimeRemaining / callTimeMax;

        timerBar.GetComponent<Image>().fillAmount = refGroup.GetWaitTimeRemaining() / waitTimeMax;

        timerClock.GetComponent<Image>().fillAmount = refGroup.GetCallTimeRemaining() / callTimeMax;

        // Show all of the key bindings
        for(int i = 0; i < refGroup.CallParticipants.Count; i++)
        {
            // Grab the caller reference
            var caller = refGroup.CallParticipants[i];

            // If the binding is empty, just ignore it
            if (caller.BoundKeyCode == KeyCode.None)
                continue;

            // Otherwise, show the binding in the text box
            bindingLetters[i].text = caller.BoundKeyCode.ToString();
        }
    }

    public void InitWithData(Call_Group _attachedGroup)
    {
        refGroup = _attachedGroup;

        // Show only the number of key binding slots that are needed for this call
        for (int i = 0; i < _attachedGroup.GetNumParticipants(); i++)
            blockingImages[i].SetActive(false);
    }

    public void OnClick()
    {
        // Find the binding manager in the scene
        Binding_Manager bindingManager = GameObject.FindObjectOfType<Binding_Manager>();

        // Pass the binding manager the attached call group so it is able to place the keybindings afterwards
        bindingManager.CallGroupToBind = this.refGroup;
    }

    public Call_Group RefGroup
    {
        get => refGroup;
    }
}
