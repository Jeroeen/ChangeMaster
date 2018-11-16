using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intervention
{
    
    public string InterventionImage { get; set; }
    public string InterventionText { get; set; }
    public string Advice { get; set; }
    public int Analytic { get; set; }
    public int Enthusiasm { get; set; }
    public int Decisive { get; set; }
    public int Empathic { get; set; }
    public int Convincing { get; set; }
    public int Creative { get; set; }
    public int ChangeKnowledge { get; set; }

    public Intervention() {}

    public Intervention(string text, string advice)
    {
        InterventionText = text;
        this.Advice = advice;
    }
}
