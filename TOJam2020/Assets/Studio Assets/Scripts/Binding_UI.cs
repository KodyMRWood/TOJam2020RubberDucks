﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class Binding_UI : MonoBehaviour
{
    //--- Public Variables ---//
    public Keyboard_Layout m_keyboardLayout;
    public Binding_Manager m_bindingManager;
    public GameObject m_callerUIPrefab;
    public Transform[] m_keyPortraitParents;
    public TextMeshProUGUI[] m_keyboardLetters;
    public Image m_swapKeyHighlight;
    public Color m_swapKeyOnColour;
    public Color m_swapKeyOffColour;
    public Key_Animator[] m_keyAnimators;



    //--- Private Variables ---//
    private KeyCode m_prevSwapKey;



    //--- Unity Methods ---//
    private void OnEnable()
    {
        // Register to the binding manager's listener
        m_bindingManager.m_OnBindingsChanged.AddListener(this.UpdateBindings);
    }

    private void OnDisable()
    {
        // Unregister from the binding manager's listener
        m_bindingManager.m_OnBindingsChanged.RemoveListener(this.UpdateBindings);
    }

    private void Awake()
    {
        // Assign all of the key animators to their corresponding keycode
        for (int i = 0; i < m_keyAnimators.Length; i++)
            m_keyAnimators[i].AttachedKeyCode = m_keyboardLayout.m_keyList[i];

        // Place all of the letters onto the keyboard;
        PlaceKeyboardLetters();
    }

    private void Update()
    {
        // Show / hide the swap mode highlight on the special key depending on if currently in swap mode
        m_swapKeyHighlight.color = (m_bindingManager.IsInSwapMode) ? m_swapKeyOnColour : m_swapKeyOffColour;

        // Manage the swap highlight on the letter keys if there was a change in the swap keys
        if (m_bindingManager.KeyToSwap != m_prevSwapKey)
        {
            // Turn off the previous swap key highlight
            // The swap highlight is currently just the portrait parent
            if (m_prevSwapKey != KeyCode.None)
            {
                int prevKeyIdx = m_keyboardLayout.GetIndexFromKeyCode(m_prevSwapKey);
                m_keyPortraitParents[prevKeyIdx].GetComponent<Image>().color = m_swapKeyOffColour;
            }
            
            // Turn on the new key highlight if there is one
            if (m_bindingManager.KeyToSwap != KeyCode.None)
            {
                int newKeyIdx = m_keyboardLayout.GetIndexFromKeyCode(m_bindingManager.KeyToSwap);
                m_keyPortraitParents[newKeyIdx].GetComponent<Image>().color = m_swapKeyOnColour;
            }

            // Store the updated key
            m_prevSwapKey = m_bindingManager.KeyToSwap;
        }
    }



    //--- Methods ---//
    public void UpdateBindings()
    {
        // Place the binding portraits
        PlaceBindings();
    }

    public void ClearAllBindings()
    {
        // Destroy all of the binding images
        foreach(var portraitHolder in m_keyPortraitParents)
        {
            // Iterate through and remove all of this parent's children
            for (int i = 0; i < portraitHolder.childCount; i++)
                Destroy(portraitHolder.GetChild(i).gameObject);
        }
    }

    public void PlaceBindings()
    {
        // Clear the existing binding images
        ClearAllBindings();

        // Get the latest bindings from the binding manager
        var newestBindings = m_bindingManager.KeyBindings;

        // Create portraits for all of the bindings
        foreach(KeyValuePair<KeyCode, Call_Individual> bindPair in newestBindings)
        {
            // Determine the index for the placement in the UI
            int uiKeyIndex = m_keyboardLayout.GetIndexFromKeyCode(bindPair.Key);

            // Toggle the main keyboard letter so that it appears if the slot is empty but hides if a caller is in it
            m_keyboardLetters[uiKeyIndex].gameObject.SetActive(bindPair.Value == null);

            // If this binding isn't set, turn the key back on and then move on
            if (bindPair.Value == null)
                continue;

            // Otherwise, determine which which caller the portrait should represent
            Call_Individual caller = bindPair.Value;

            // Now, spawn a new portrait on the right UI key and set it up with the caller object
            GameObject portraitObj = Instantiate(m_callerUIPrefab, m_keyPortraitParents[uiKeyIndex]);
            Call_Individual_UI uiComp = portraitObj.GetComponent<Call_Individual_UI>();
            uiComp.InitWithData(caller);
        }
    }

    public void PlaceKeyboardLetters()
    {
        // Use the assigned keyboard layout and fill in the letters on the keyboard accordingly
        for(int i = 0; i < m_keyboardLetters.Length; i++)
        {
            // Grab the keycode that is associated with this key
            KeyCode keyCode = m_keyboardLayout.m_keyList[i];

            // Convert it to a string
            string keyString = keyCode.ToString();

            // Place it into the UI object
            m_keyboardLetters[i].text = keyString;
        }
    }
}
