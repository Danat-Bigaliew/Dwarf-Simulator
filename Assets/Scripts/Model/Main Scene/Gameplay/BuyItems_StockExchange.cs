using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BuyItems_StockExchange : MonoBehaviour
{
    [SerializeField] private WebSocketConnect webSocketConnect;

    [SerializeField] private string taskBuy = "buyInStockExchange";
    [SerializeField] private string taskCell = "cellInStockExchange";

    private Transform content;

    private PlayerDataOnSession playerDataOnSession;
    private ClicksController clicksController;

    [Inject]
    public void Construct(PlayerDataOnSession PlayerDataOnSession, ClicksController ClicksController)
    {
        playerDataOnSession = PlayerDataOnSession;
        clicksController = ClicksController;
    }

    public void SetupBuyItems_StockExchange()
    {
        content = GetComponent<Transform>();

        SubscriptionToPurchase();
    }

    private void SubscriptionToPurchase()
    {
        int tempIndex = 0;

        foreach (Transform child in content)
        {
            Transform activeMenu = child.GetChild(2).GetComponent<Transform>();
            Transform buttons = activeMenu.GetChild(1).GetChild(1).GetComponent<Transform>();

            Button buyButton = buttons.GetChild(0).GetComponent<Button>();
            Button cellButton = buttons.GetChild(1).GetComponent<Button>();

            int index = tempIndex;

            buyButton.onClick.AddListener(() => webSocketConnect.SetVariable(playerDataOnSession.playerKey, Convert.ToString(index + 1), taskBuy));
            cellButton.onClick.AddListener(() => webSocketConnect.SetVariable(playerDataOnSession.playerKey, Convert.ToString(index + 1), taskCell));

            ++tempIndex;
        }
    }
}