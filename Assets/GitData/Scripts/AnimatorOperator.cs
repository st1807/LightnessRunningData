using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorOperator : StateMachineBehaviour
{
    public string stateName;
    public bool stateSet;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        animator.SetBool(stateName,stateSet);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        animator.SetBool(stateName,!stateSet);
    }
}
