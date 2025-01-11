using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BuyItems_forge : MonoBehaviour
{
    [SerializeField] private WebSocketConnect webSocketConnect;

    [SerializeField] private string task = "buyInForge";

    private Transform content;

    private PlayerDataOnSession playerDataOnSession;
    private ClicksController clicksController;

    [Inject]
    public void Construct(PlayerDataOnSession PlayerDataOnSession, ClicksController ClicksController)
    {
        playerDataOnSession = PlayerDataOnSession;
        clicksController = ClicksController;
    }

    public void SetupBuyItems_forge()
    {
        content = GetComponent<Transform>();

        SubscriptionToPurchase();
    }

    private void SubscriptionToPurchase()
    {
        int tempIndex = 0;

        foreach(Transform child in content)
        {
            int index = tempIndex;

            Button buttonItem = child.GetChild(4).GetComponent<Button>();
            buttonItem.onClick.AddListener(() =>
            {
                clicksController.ButtonClickAudio();
                webSocketConnect.SetVariable(playerDataOnSession.playerKey, Convert.ToString(index + 1), task);
            });

            ++tempIndex;
        }
    }
}