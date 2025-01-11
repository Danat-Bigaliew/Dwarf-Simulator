using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TavernClickState : IState
{
    private List<Button> buttonsArray = new List<Button>();
    private List<Animator> animatorsArray = new List<Animator>();

    private Dictionary<string, float> animationLength = new Dictionary<string, float>();

    private string currentAnimation = "Tavern_Click";

    public TavernClickState(
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
        buttonsArray[2].interactable = false;
        monobehaivor.StartCoroutine(AnimationController(animationsLength[currentAnimation]));
    }

    private IEnumerator AnimationController(float animationDuration)
    {
        AnimatorsArray[2].SetTrigger("ToClick");

        yield return new WaitForSeconds(animationDuration);
        Debug.Log("Shaft_Click animation finished. Returning to Shaft_Idle...");

        buttonsArray[2].interactable = true;

        stateMachine.ChangeState(new IdleState(stateMachine, animationsLength, AnimatorsArray, buttonsArray, monobehaivor));
    }
}