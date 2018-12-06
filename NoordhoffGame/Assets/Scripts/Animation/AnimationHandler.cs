using UnityEngine;

namespace Assets.Scripts.Animation
{
	public class AnimationHandler : MonoBehaviour
	{
		[SerializeField] private Animator animator = null;
    
		public void ToggleAnimation(string condition)
		{
			animator.SetBool(condition, !animator.GetBool(condition));
		}

		public void PlayAnimation(string trigger)
		{
			animator.SetTrigger(trigger);
		}
	}
}
