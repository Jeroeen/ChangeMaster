﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void ToggleAnimation()
    {
        animator.SetBool("isPlaying", !animator.GetBool("isPlaying"));
    }

    public void PlayAnimation()
    {
        animator.SetTrigger("triggerGlijbaan");
    }
}
