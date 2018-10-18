using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterventionList 
{

    public Intervention[] interventions { get; set; }
    
    public InterventionList()
    {

    }
    public InterventionList(Intervention[] Interventions)
    {
        interventions = Interventions;
    }

}
