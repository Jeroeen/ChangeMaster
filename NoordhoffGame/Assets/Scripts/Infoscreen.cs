using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Infoscreen : MonoBehaviour
{
    public Button settingsbutton;
    public CanvasGroup Info;
    public GameObject panel;
    public Sprite Img1;
    public Sprite Img2;

    private bool disable = false;
    private Vector2 pos = new Vector2(0.0f, -5.0f);
    private ScrollRect infoScroll;


    // Start is called before the first frame update
    void Start()
    {
        infoScroll = Info.GetComponentInChildren<ScrollRect>();
        Sprite[] Images = new Sprite[] { Img1, Img2, Img1, Img2, Img1, Img2 };
        string[] Text = new string[] { "Ik ben het er niet mee eens", "Ik ben het er wel mee eens", "Ik ben het er een beetje mee eens", "Ik ben het er heel fanatiek mee eens", "Ik ben het er helemaal niet mee eens", "Mij maakt het eigenlijk niet uit"};
        List<GameObject> Panels = new List<GameObject>();
        for (int i = 0; i < Images.Length; i++)
        {
            Panels.Add(Instantiate(panel, infoScroll.content.transform));
            Text[] testText = Panels[i].GetComponentsInChildren<Text>();
            testText[0].text = Text[i];
            Image[] testImage = Panels[i].GetComponentsInChildren<Image>();
            testImage[1].sprite = Images[i];
            RectTransform[] rTest = Panels[i].GetComponents<RectTransform>();
            rTest[0].anchoredPosition = pos;
            RectTransform testScroll = infoScroll.content.GetComponent<RectTransform>();
            if(i >= 3)
            {
            testScroll.sizeDelta = new Vector2(testScroll.sizeDelta.x, testScroll.sizeDelta.y + 110);
                testScroll.anchoredPosition = new Vector2(testScroll.anchoredPosition.x, testScroll.anchoredPosition.y - 60);

            }
            pos = new Vector2(pos.x, pos.y - 110.0f);

        }
        
    }
    //analog = !analog;
    public void enableInfo()
    {
        if (!disable)
        {
            if(Info.interactable)
            {
                Info.interactable = false;
                Info.alpha = 0;
                Info.blocksRaycasts = false;
            }
            else
            {
                Info.interactable = true;
                Info.alpha = 1;
                Info.blocksRaycasts = true;
            }
            settingsbutton.interactable = !settingsbutton.IsInteractable();
        }
    }

    public void switchDisable()
    {
        disable = !disable;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
