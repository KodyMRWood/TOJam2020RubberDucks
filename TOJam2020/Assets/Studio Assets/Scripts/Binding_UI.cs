﻿using UnityEngine;
using System.Collections.Generic;

public class Binding_UI : MonoBehaviour
{
    //--- Public Variables ---//
    public Keyboard_Layout m_keyboardLayout;
    public Binding_Manager m_bindingManager;
    public GameObject m_callerUIPrefab;
    public Transform[] m_keyPortraitParents;



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
            // If this binding isn't set, just move on
            if (bindPair.Value == null)
                continue;

            // Otherwise, determine which key should hold the portrait and which caller the portrait should represent
            int uiKeyIndex = m_keyboardLayout.GetIndexFromKeyCode(bindPair.Key);
            Call_Individual caller = bindPair.Value;

            // Now, spawn a new portrait on the right UI key and set it up with the caller object
            GameObject portraitObj = Instantiate(m_callerUIPrefab, m_keyPortraitParents[uiKeyIndex]);
            Call_Individual_UI uiComp = portraitObj.GetComponent<Call_Individual_UI>();
            uiComp.InitWithData(caller);
        }
    }
}
