using Assets.Scripts;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RetrieveJson
{
    public DialogueItem LoadJson(string nameOfPartner, string stage, int dialogueCount)
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

        return item;
    }

}
