using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct info
{
    public string Image;
    public string Text;
}

public class InfoList
{
    public info[] InformationList;

    public InfoList()
    {

    }
    public InfoList(info[] Information)
    {
        InformationList = Information;
    }
}
