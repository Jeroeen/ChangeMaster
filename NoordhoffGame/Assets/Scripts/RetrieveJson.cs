using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RetrieveJson
{
    public InterventionList LoadJsonInterventions(int level)
    {
        string path;

        path = Application.dataPath + "/StreamingAssets/InterventionFiles/" + "InterventionsLevel_" + level + ".json";
        

        string jsonString = File.ReadAllText(path);
        InterventionList item = JsonMapper.ToObject<InterventionList>(jsonString);

        return item;
    }

    public InfoList LoadJsonInformation(int level)
    {
        string path;

        path = Application.dataPath + "/StreamingAssets/InformationFiles/" + "InformationLevel_" + level + ".json";


        string jsonString = File.ReadAllText(path);
        InfoList item = JsonMapper.ToObject<InfoList>(jsonString);

        return item;
    }

}
