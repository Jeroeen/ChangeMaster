using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intervention
{
    
    public string intervention { get; set; }

    public string advice { get; set; }


    public int Analytisch { get; set; }

    public int Enthousiasmerend { get; set; }

    public int Besluitvaardig { get; set; }

    public int Empathisch { get; set; }

    public int Overtuigend { get; set; }

    public int Creatief { get; set; }

    public int Kennis_veranderkunde { get; set; }

    public Intervention()
    {

    }
    public Intervention(string Intervention, string Advice)
    {
        intervention = Intervention;
        advice = Advice;

    }
}
