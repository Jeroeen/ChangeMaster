﻿using Assets.Scripts;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Dialogue;
using UnityEngine;

public class RetrieveJson
{
    public DialogueItem LoadJsonDialogue(string nameOfPartner, string stage, int dialogueCount)
    {
        string path;
        if (dialogueCount < 0)
        {
            path = "DialogueFiles/" + stage + "/" + nameOfPartner;
        }
        else
        {
            path = "DialogueFiles/" + stage + "/" + nameOfPartner + "-" + dialogueCount;
        }

        Debug.Log(dialogueCount);

        TextAsset asset = Resources.Load(path) as TextAsset;
        string jsonString = asset.ToString();

        DialogueItem item = JsonMapper.ToObject<DialogueItem>(jsonString);
        item.ReplaceName();

        return item;
    }
	

    public InterventionList LoadJsonInterventions(int level)
    {
        string path = "InterventionFiles/InterventionsLevel_" + level;

        TextAsset asset = Resources.Load(path) as TextAsset;
        string jsonString = asset.ToString();
        InterventionList item = JsonMapper.ToObject<InterventionList>(jsonString);

        return item;
    }

    public InfoList LoadJsonInformation(int level)
    {
        string path = "InformationFiles/InformationLevel_" + level;


        TextAsset asset = Resources.Load(path) as TextAsset;
        string jsonString = asset.ToString();
        InfoList item = JsonMapper.ToObject<InfoList>(jsonString);
        for (int i = 0; i < item.InformationList.Length; i++)
        {
            item.InformationList[i].Found = false;
        }
        return item;
    }

}
