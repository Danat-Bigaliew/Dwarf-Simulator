using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ShaftGameplay : MonoBehaviour
{
    [SerializeField] private Button shaft;
    [SerializeField] private Button market;
    [SerializeField] private Button tavern;

    [SerializeField] private WebSocketConnect webSocketConnect;

    private string task = "MainGamePlay";

    private PlayerDataOnSession playerDataOnSession;
    [Inject]
    private void Construct(PlayerDataOnSession PlayerDataOnSession)
    {
        playerDataOnSession = PlayerDataOnSession;
    }

    public void Setup_MainGameplay()
    {
        shaft.onClick.AddListener(() => webSocketConnect.SetVariable(playerDataOnSession.playerKey, "shaft", task));
        market.onClick.AddListener(() => webSocketConnect.SetVariable(playerDataOnSession.playerKey, "market", task));
        tavern.onClick.AddListener(() => webSocketConnect.SetVariable(playerDataOnSession.playerKey, "tavern", task));
    }
}