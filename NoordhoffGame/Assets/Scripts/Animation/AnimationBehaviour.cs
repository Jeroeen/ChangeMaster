using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Initialization;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnimationBehaviour : StateMachineBehaviour
{
    private Physics2DRaycaster animationRaycaster;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animationRaycaster == null)
        {
            animationRaycaster = Camera.main.GetComponents<Physics2DRaycaster>().First();
        }
        animationRaycaster.enabled = false;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animationRaycaster.enabled = true;
    }
}
