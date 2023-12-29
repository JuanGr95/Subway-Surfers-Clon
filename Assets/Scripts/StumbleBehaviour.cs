using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StumbleBehaviour : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName("StumbleSideLeft") || stateInfo.IsName("StumbleSideRight"))
        {
            PlayerController playerController = animator.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.IsStumbleTransitionComplete = true;
            }
        }
    }
}
