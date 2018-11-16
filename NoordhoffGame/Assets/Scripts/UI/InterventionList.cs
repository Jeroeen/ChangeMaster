namespace Assets.Scripts.UI
{
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
}
