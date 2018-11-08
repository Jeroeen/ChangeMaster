using Assets.Scripts;
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
            path = "DialogueFiles/" + nameOfPartner + "-" + stage;
        }
        else
        {
            path = "DialogueFiles/" + nameOfPartner + "-" + stage + "-" + dialogueCount;
        }

        TextAsset asset = Resources.Load(path) as TextAsset;
        string jsonString = asset.ToString();

        DialogueItem item = JsonMapper.ToObject<DialogueItem>(jsonString);
        item.ReplaceName();

        return item;
    }
	

    public InterventionList LoadJsonInterventions(string level)
    {
        string path = "InterventionFiles/Interventions" + level;

        TextAsset asset = Resources.Load(path) as TextAsset;
        string jsonString = asset.ToString();
        InterventionList item = JsonMapper.ToObject<InterventionList>(jsonString);

        return item;
    }

    public InfoList LoadJsonInformation(string level)
    {
        string path = "InformationFiles/Information" + level;


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
