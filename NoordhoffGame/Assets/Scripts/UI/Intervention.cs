using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intervention
{
    
    public string InterventionText { get; set; }
    public string Advice { get; set; }
    public int Analytisch { get; set; }
    public int Enthousiasmerend { get; set; }
    public int Besluitvaardig { get; set; }
    public int Empathisch { get; set; }
    public int Overtuigend { get; set; }
    public int Creatief { get; set; }
    public int Kennis_veranderkunde { get; set; }

    public Intervention()
    {}
    public Intervention(string Text, string Advice)
    {
        InterventionText = Text;
        this.Advice = Advice;
    }
}
