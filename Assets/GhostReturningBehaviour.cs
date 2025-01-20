using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostReturningBehaviour : StateMachineBehaviour
{
    private GhostMovement _ghostMovement;
    private int _nearestCheckpoint;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _ghostMovement = animator.GetComponent<GhostMovement>();
        _nearestCheckpoint = _ghostMovement.GetNearestCheckpoint();
        _ghostMovement.CalculatePathToCheckpoint(_nearestCheckpoint);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector3.Distance(animator.transform.position, _ghostMovement.checkpoints[_nearestCheckpoint].transform.position) < 0.1f)
        {
            animator.SetTrigger("haveReturned");
            return;
        }

        _ghostMovement.FollowPath();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _ghostMovement.SetCheckpoint(_nearestCheckpoint);
        animator.ResetTrigger("haveReturned");
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
