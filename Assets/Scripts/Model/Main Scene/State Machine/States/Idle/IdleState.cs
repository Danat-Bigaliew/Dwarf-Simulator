using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IdleState : IState
{
    private List<Animator> animatorsArray = new List<Animator>();

    public IdleState(
        StateMachine StateMachine,
        Dictionary<string, float> animationsLength,
        List<Animator> AnimatorsArray,
        List<Button> ButtonsArray,
        MonoBehaviour MonoBehaivor) :

        base(StateMachine, animationsLength, AnimatorsArray, ButtonsArray, MonoBehaivor)
    {
        animatorsArray = AnimatorsArray;
    }

    public override void Enter()
    {
        Debug.Log("Entering Shaft_Idle state");
        foreach (Animator animator in AnimatorsArray)
        {
            animator.SetTrigger("ToIdle");
        }
    }
}