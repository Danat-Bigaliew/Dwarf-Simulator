using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class IState
{
    protected readonly StateMachine stateMachine;
    protected readonly MonoBehaviour monobehaivor;

    public List<Animator> AnimatorsArray { get; private set; } = new List<Animator>();
    public List<Button> ButtonsArray { get; private set; } = new List<Button>();

    public Dictionary<string, float> animationsLength { get; private set; } = new Dictionary<string, float>();

    public IState(
        StateMachine StateMachine, 
        Dictionary<string, float> animationsLength,
        List<Animator> animatorsArray, 
        List<Button> buttonsArray, 
        MonoBehaviour Monobehaivor
        )
    {
        stateMachine = StateMachine;
        monobehaivor = Monobehaivor;
        this.animationsLength = animationsLength;
        AnimatorsArray = animatorsArray;
        ButtonsArray = buttonsArray;
    }

    public virtual void Enter() { }
    public virtual void Execute() { }// Выполняет работу системного метода Update
    public virtual void Exit() { }
}