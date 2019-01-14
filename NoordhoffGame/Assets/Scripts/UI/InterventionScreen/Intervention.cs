namespace Assets.Scripts.UI.InterventionScreen
{
	public class Intervention
	{
    
    public string InterventionImage { get; set; }
    public string InterventionText { get; set; }
    public string Hint { get; set; }
    public string Consequence { get; set; }
    public string Advice { get; set; }
    public int Analytic { get; set; }
    public int Approach { get; set; }
    public int Ownership { get; set; }
    public int Facilitating { get; set; }
    public int Communication { get; set; }

		public Intervention() { }

		public Intervention(string text, string advice)
		{
			InterventionText = text;
			this.Advice = advice;
		}
	}
}
