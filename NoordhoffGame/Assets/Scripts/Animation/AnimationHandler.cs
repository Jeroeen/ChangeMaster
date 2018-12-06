using UnityEngine;

namespace Assets.Scripts.Animation
{
	public class AnimationHandler : MonoBehaviour
	{
		[SerializeField] private Animator animator = null;
    
		public void ToggleAnimation()
		{
			animator.SetBool("isPlaying", !animator.GetBool("isPlaying"));
		}

		public void PlayAnimation()
		{
			animator.SetTrigger("triggerGlijbaan");
		}
	}
}
