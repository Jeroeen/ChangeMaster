using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterventionList 
{

    public Intervention[] Interventions { get; set; }
    
    public InterventionList()
    {}
    public InterventionList(Intervention[] Interventions)
    {
        this.Interventions = Interventions;
    }

}
