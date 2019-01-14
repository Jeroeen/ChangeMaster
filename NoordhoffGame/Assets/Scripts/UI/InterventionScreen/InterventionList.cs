namespace Assets.Scripts.UI.InterventionScreen
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
