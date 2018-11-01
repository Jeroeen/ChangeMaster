using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitializeBaseview : MonoBehaviour
{
    [SerializeField] private CharModel charModel;
    [SerializeField] private InitiateDialogue initiateDialogue;
    [SerializeField] private GameObject dialogue;

    [SerializeField] private Transition transition;

    void Start()
    {
        initiateDialogue.Initialize(charModel.NameOfPartner, charModel.Stage, charModel.DialogueCount);
        dialogue.SetActive(true);
    }

    void Update()
    {
        if (dialogue.activeSelf) return;

        if (!transition.transform.gameObject.activeSelf)
        {
            transition.transform.gameObject.SetActive(true);
        }
        else
        {
            if (transition.FadeOut())
            {
                SceneManager.LoadScene(2);
            }
        }
    }
}
