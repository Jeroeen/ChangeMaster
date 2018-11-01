using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Info
{
    public string Image;
    public string Text;
}

public class InfoList
{
    public Info[] InformationList;

    public InfoList()
    {

    }

    public InfoList(Info[] Information)
    {
        InformationList = Information;
    }
}
