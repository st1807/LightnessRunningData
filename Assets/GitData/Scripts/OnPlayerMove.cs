using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlayerMove : StateMachineBehaviour
{
    AudioClip stepClip;
    AudioSource moveAudio;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        if (!moveAudio)
        {
            moveAudio = GameObject.Find("Bow_charactor/sounds").GetComponent<AudioSource>();
        }

        if (stepClip==null)
        {
            stepClip = Resources.Load<AudioClip>("Sounds/Classic Footstep SFX/Forest ground/Forest_ground_step2");
            if (stepClip)
            {
                moveAudio.clip = stepClip;
            }
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        if (animator.GetFloat("speed") >= 0.5f)
        {
            if (!moveAudio.isPlaying)
            {
                moveAudio.Play();
            }
        }
        else
        {
            moveAudio.Stop();
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        moveAudio.Stop();
    }
}
