using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intervention
{
    
    public string InterventionText { get; set; }
    public string Advice { get; set; }
    public int Analytic { get; set; }
    public int Enthusiasm { get; set; }
    public int Decisive { get; set; }
    public int Empatic { get; set; }
    public int Convincing { get; set; }
    public int Creative { get; set; }
    public int ChangeKnowledge { get; set; }

    public Intervention()
    {}
    public Intervention(string Text, string Advice)
    {
        InterventionText = Text;
        this.Advice = Advice;
    }
}
