using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketClickState : IState
{
    private List<Button> buttonsArray = new List<Button>();
    private List<Animator> animatorsArray = new List<Animator>();

    private Dictionary<string, float> animationLength = new Dictionary<string, float>();

    private string currentAnimation = "Market_Click";

    public MarketClickState(
        StateMachine StateMachine,
        Dictionary<string, float> animationsLength,
        List<Animator> AnimatorsArray,
        List<Button> ButtonsArray,
        MonoBehaviour MonoBehaivor) :

        base(StateMachine, animationsLength, AnimatorsArray, ButtonsArray, MonoBehaivor)
    {
        animationLength = animationsLength;
        animatorsArray = AnimatorsArray;
        buttonsArray = ButtonsArray;
    }

    public override void Enter()
    {
        Debug.Log("Entering Shaft_Click state");
        buttonsArray[1].interactable = false;
        monobehaivor.StartCoroutine(AnimationController(animationsLength[currentAnimation]));
    }

    private IEnumerator AnimationController(float animationDuration)
    {
        AnimatorsArray[1].SetTrigger("ToClick");

        yield return new WaitForSeconds(animationDuration);
        Debug.Log("Shaft_Click animation finished. Returning to Shaft_Idle...");

        buttonsArray[1].interactable = true;

        stateMachine.ChangeState(new IdleState(stateMachine, animationsLength, AnimatorsArray, buttonsArray, monobehaivor));
    }
}