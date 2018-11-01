using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InitializeBaseview : MonoBehaviour
{
    [SerializeField] private CharModel charModel;
    [SerializeField] private InitiateDialogue initiateDialogue;
    [SerializeField] private GameObject dialogue;

    [SerializeField] private Transition transition;

    [SerializeField] private Button infoButton;
    [SerializeField] private Button settingsButton;

    private bool hasOpenedDialogue;
    

    void Update()
    {
        if (dialogue.activeSelf || !hasOpenedDialogue) return;

        if (!transition.transform.gameObject.activeSelf)
        {
            transition.transform.gameObject.SetActive(true);
        }
        else
        {
            if (transition.FadeOut())
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    public void ShowDialogCaptain()
    {
        infoButton.interactable = !infoButton.IsInteractable();
        settingsButton.interactable = !settingsButton.IsInteractable();
        initiateDialogue.Initialize(charModel.NameOfPartner, charModel.Stage, charModel.DialogueCount);
        dialogue.SetActive(true);
        hasOpenedDialogue = true;
    }

   
}
