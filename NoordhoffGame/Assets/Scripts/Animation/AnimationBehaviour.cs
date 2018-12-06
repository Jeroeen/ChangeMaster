using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Animation
{
	public class AnimationBehaviour : StateMachineBehaviour
	{
		private Physics2DRaycaster animationRaycaster;

		// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (animationRaycaster == null)
			{
				animationRaycaster = Camera.main.GetComponents<Physics2DRaycaster>().First();
			}
			animationRaycaster.enabled = false;
		}

		// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			animationRaycaster.enabled = true;
		}
	}
}
