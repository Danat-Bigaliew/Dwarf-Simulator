using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ShaftStateController : MonoBehaviour
{
    [SerializeField] private WebSocketConnect webSocketConnect;

    private string task = "MainGamePlay";

    private Transform content;
    private StateMachine stateMachine;
    private List<Button> buttonsArray = new List<Button>();
    private List<Animator> animatorsArray = new List<Animator>();
    private Dictionary<string, float> animationsLength = new Dictionary<string, float>();

    private IState idleState;
    private IState shaftClickState;
    private IState marketClickState;
    private IState tavernClickState;

    private PlayerDataOnSession playerDataOnSession;
    private ClicksController clicksController;

    [Inject]
    private void Construct(PlayerDataOnSession PlayerDataOnSession, ClicksController ClicksController)
    {
        playerDataOnSession = PlayerDataOnSession;
        clicksController = ClicksController;
    }

    private void Start()
    {
        SetupVariable();
        SetupAnimations();

        stateMachine = new StateMachine();

        idleState = new IdleState(stateMachine, animationsLength, animatorsArray, buttonsArray, this);
        shaftClickState = new ShaftClickState(stateMachine, animationsLength, animatorsArray, buttonsArray, this);
        marketClickState = new MarketClickState(stateMachine, animationsLength, animatorsArray, buttonsArray, this);
        tavernClickState = new TavernClickState(stateMachine, animationsLength, animatorsArray, buttonsArray, this);

        stateMachine.ChangeState(idleState);

        SetupButtonListeners();
    }

    private void SetupVariable()
    {
        content = GetComponent<Transform>();

        Transform shaftContainer = content.GetChild(0).GetComponent<Transform>();
        Transform marketContainer = content.GetChild(1).GetComponent<Transform>();
        Transform tavernContainer = content.GetChild(2).GetComponent<Transform>();

        buttonsArray.Add(shaftContainer.GetComponent<Button>());
        animatorsArray.Add(shaftContainer.GetComponent<Animator>());

        buttonsArray.Add(marketContainer.GetComponent<Button>());
        animatorsArray.Add(marketContainer.GetComponent<Animator>());

        buttonsArray.Add(tavernContainer.GetComponent<Button>());
        animatorsArray.Add(tavernContainer.GetComponent<Animator>());
    }

    private void SetupAnimations()
    {
        foreach (Animator animator in animatorsArray)
        {
            AnimationClip[] animations = animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in animations)
            {
                animationsLength.Add(clip.name, clip.length);
            }
        }
    }

    private void SetupButtonListeners()
    {
        for (int i = 0; i < buttonsArray.Count; i++)
        {
            int index = i;
            buttonsArray[i].onClick.AddListener(() => HandleButtonClick(index));
        }
    }

    private void HandleButtonClick(int index)
    {
        switch (index)
        {
            case 0:
                clicksController.ButtonClickAudioInMainScene();
                webSocketConnect.SetVariable(playerDataOnSession.playerKey, "shaft", task);
                break;
            case 1:
                clicksController.ButtonClickAudioInMainScene();
                webSocketConnect.SetVariable(playerDataOnSession.playerKey, "market", task);
                break;
            case 2:
                clicksController.ButtonClickAudioInMainScene();
                webSocketConnect.SetVariable(playerDataOnSession.playerKey, "tavern", task);
                break;
        }
    }

    public void SetupShaftClickAnimation()
    {
        stateMachine.ChangeState(shaftClickState);
    }

    public void SetupMarketClickAnimation()
    {
        stateMachine.ChangeState(marketClickState);
    }

    public void SetupTavernClickAnimation()
    {
        stateMachine.ChangeState(tavernClickState);
    }
}