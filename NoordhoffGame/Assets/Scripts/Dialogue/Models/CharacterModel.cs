using UnityEngine;

namespace Assets.Scripts.Dialogue.Models
{
	public class CharacterModel : MonoBehaviour
	{
		public string Stage;

	    private int dialogueCount;

	    [HideInInspector]
	    public int DialogueCount
	    {
	        get => AmountOfDialogues > 1 ? 0 : -1;
	        set => dialogueCount = value;
	    }

	    public int AmountOfDialogues;
		public string NameOfPartner;
	}
}
