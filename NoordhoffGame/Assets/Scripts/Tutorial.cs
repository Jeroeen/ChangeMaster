using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
	[SerializeField] private GameObject settingsButton;
	[SerializeField] private GameObject infoButton;
	[SerializeField] private GameObject interventionScreenButton;
	[SerializeField] private GameObject coinCount;
	[SerializeField] private GameObject interactables;


    // Start is called before the first frame update
    void Start()
    {
        settingsButton.SetActive(false);
		infoButton.SetActive(false);
		interventionScreenButton.SetActive(false);
		coinCount.SetActive(false);
		interactables.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
