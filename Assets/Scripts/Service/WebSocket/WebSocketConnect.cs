using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class WebSocketConnect : MonoBehaviour
{
    [SerializeField] private MarketSceneUI marketSceneUI;
    [SerializeField] private StockExchangeSceneUI stockExchangeSceneUI;
    [SerializeField] private ProgressBars_UI progressBarsUI;
    [SerializeField] private Counters_Animation countersAnimation;
    [SerializeField] private ShaftStateController shaftStateController;
    [SerializeField] private MoveContextMenu moveContextMenu;

    private WebSocket ws;

    private string key;
    private string indexItem;
    private string task;
    Dictionary<string, Dictionary<string, string>> mainMessage = new Dictionary<string, Dictionary<string, string>>();
    Dictionary<string, string> message = new Dictionary<string, string>();

    private ConcurrentQueue<Action> mainThreadActions = new ConcurrentQueue<Action>();

    public void SetVariable(string playerKey, string indexItem, string currentTask)
    {
        Debug.Log($"current_task : {currentTask}");

        key = playerKey;
        this.indexItem = indexItem;
        task = currentTask;

        SendMessageToServer();
    }

    public void SendMessageToServer()
    {
        message = new Dictionary<string, string>
            {
                { task, indexItem }
            };
        mainMessage = new Dictionary<string, Dictionary<string, string>>
            {
                { key, message }
            };

        if (ws != null && ws.IsAlive)
        {
            string jsonMessage = JsonConvert.SerializeObject(mainMessage);
            ws.Send(jsonMessage);
            Debug.Log($"Сообщение отправлено на сервер: {jsonMessage}");
        }
        else
        {
            Debug.LogWarning("WebSocket соединение не активно.");
        }
    }

    public void ConnectToWebSocketServer()
    {
        string serverUrl = "ws://127.0.0.1:8001/ws";
        ws = new WebSocket(serverUrl);

        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("Соединение установлено!");
        };

        ws.OnMessage += (sender, e) =>
        {
            string answerFromServer = e.Data;

            Debug.Log($"answerFromServer : {answerFromServer}");

            BuyInMarkets(answerFromServer);
            ClickGameplay(answerFromServer);
        };

        ws.OnError += (sender, e) =>
        {
            Debug.LogError($"Ошибка WebSocket: {e.Message}");
        };

        ws.OnClose += (sender, e) =>
        {
            Debug.Log($"Соединение закрыто. Код: {e.Code}, Причина: {e.Reason}");
        };

        Debug.Log("Попытка подключения...");
        ws.ConnectAsync();
    }

    private void ClickGameplay(string answerFromServer)
    {
        try
        {
            JObject jsonResponse = JObject.Parse(answerFromServer);

            const string MessageAboutFatigue = "Покупка невозможна, вы устали";
            const string MessageAboutLackOfMoney = "Покупка невозможна, у вас мало голды";

            string message = "";
            string gold = "";
            string diamond = "";
            int happiness = 0;
            int strength = 0;
            int eloquence = 0;

            switch (indexItem)
            {
                case "shaft":
                    message = jsonResponse["message"]?.Value<string>() ?? "NULL";

                    switch (message)
                    {
                        case "Покупка совершена":
                            SetupShaftAnimation();

                            diamond = jsonResponse["player_diamond"]?.Value<string>() ?? "NULL";
                            happiness = jsonResponse["happiness"]?.Value<int>() ?? 0;
                            strength = jsonResponse["strength"]?.Value<int>() ?? 0;
                            eloquence = jsonResponse["eloquence"]?.Value<int>() ?? 0;

                            UpdatingUIData(diamond, happiness, strength, eloquence);
                            break;
                        case "Покупка невозможна, вы устали":
                            UpdatingUIData(MessageAboutFatigue);
                            break;
                    }
                    break;
                case "market":
                    message = jsonResponse["message"]?.Value<string>() ?? "NULL";

                    switch (message)
                    {
                        case "Покупка совершена":
                            SetupMarketAnimation();

                            diamond = jsonResponse["player_diamond"]?.Value<string>() ?? "NULL";
                            gold = jsonResponse["player_gold"]?.Value<string>() ?? "NULL";
                            diamond = jsonResponse["player_diamond"]?.Value<string>() ?? "NULL";
                            happiness = jsonResponse["happiness"]?.Value<int>() ?? 0;
                            strength = jsonResponse["strength"]?.Value<int>() ?? 0;
                            eloquence = jsonResponse["eloquence"]?.Value<int>() ?? 0;

                            UpdatingUIData(diamond, happiness, strength, eloquence);
                            break;
                        case "Покупка невозможна, вы устали":
                            UpdatingUIData(MessageAboutFatigue);
                            break;
                    }
                    break;
                case "tavern":
                    switch (message)
                    {
                        case "Покупка совершена":
                            SetupTavernAnimation();

                            gold = jsonResponse["player_gold"]?.Value<string>() ?? "NULL";
                            happiness = jsonResponse["happiness"]?.Value<int>() ?? 0;
                            strength = jsonResponse["strength"]?.Value<int>() ?? 0;
                            eloquence = jsonResponse["eloquence"]?.Value<int>() ?? 0;

                            UpdatingUIData(diamond, happiness, strength, eloquence);
                            break;
                        case MessageAboutLackOfMoney:
                            UpdatingUIData(MessageAboutFatigue);
                            break;
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка при разборе JSON: {ex.Message}");
        }
    }
    private void BuyInMarkets(string answerFromServer)
    {
        try
        {
            JObject jsonResponse = JObject.Parse(answerFromServer);
            Debug.Log($"task: {task}");

            string gold = "";
            int quantityProduct = 0;
            string priceProduct = "";
            string payButtonText = "";
            string errorMessage = "";

            switch (task)
            {
                case "buyInForge":
                    gold = jsonResponse["player_gold"]?.Value<string>() ?? "NULL";
                    priceProduct = jsonResponse["price"]?.Value<string>() ?? "NULL";
                    payButtonText = jsonResponse["ui_payButton"]?.Value<string>() ?? "NULL";

                    switch (priceProduct)
                    {
                        case "NULL":
                            errorMessage = jsonResponse["error"]?.Value<string>() ?? "Что-то не так";
                            Debug.Log($"errorMessage : {errorMessage}");
                            break;
                        default:
                            UpdatingUIData(Convert.ToInt32(indexItem), gold, payButtonText, priceProduct);
                            break;
                    }
                    break;
                case "buyInStockExchange":
                    quantityProduct = jsonResponse["quantity_product"]?.Value<int>() ?? 0;
                    gold = jsonResponse["player_gold"]?.Value<string>() ?? "NULL";

                    UpdatingUIData(Convert.ToInt32(indexItem), gold, quantityProduct);
                    break;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка при разборе JSON: {ex.Message}");
        }
    }

    private void UpdatingUIData(int indexCurrentButton, string gold, string payButtonText, string priceProduct)
    {
        mainThreadActions.Enqueue(() =>
        {
            marketSceneUI.UpdateForgeData(indexCurrentButton, payButtonText, priceProduct);
            countersAnimation.UpdateGoldVariable(gold);
        });
    }
    private void UpdatingUIData(int indexCurrentButton, string gold, int quantityProduct)
    {
        mainThreadActions.Enqueue(() =>
        {
            stockExchangeSceneUI.UpdateStockExchangeData(indexCurrentButton, quantityProduct);
            countersAnimation.UpdateGoldVariable(gold);
        });
    }
    private void UpdatingUIData(string diamond, float happiness, float strength, float eloquence)
    {
        mainThreadActions.Enqueue(() =>
        {
            countersAnimation.UpdateDiamondCounter(diamond);
            progressBarsUI.UpdateProgressBars(happiness, strength, eloquence);
        });
    }
    private void UpdatingUIData(string message)
    {
        mainThreadActions.Enqueue(() =>
        {
            StartCoroutine(moveContextMenu.AnimationContent(message));
        });
    }

    private void SetupShaftAnimation()
    {
        mainThreadActions.Enqueue(() =>
        {
            shaftStateController.SetupShaftClickAnimation();
        });
    }
    private void SetupMarketAnimation()
    {
        mainThreadActions.Enqueue(() =>
        {
            shaftStateController.SetupMarketClickAnimation();
        });
    }
    private void SetupTavernAnimation()
    {
        mainThreadActions.Enqueue(() =>
        {
            shaftStateController.SetupTavernClickAnimation();
        });
    }

    private void Update()
    {
        while (mainThreadActions.TryDequeue(out var action))
        {
            action?.Invoke();
        }
    }

    private void OnApplicationQuit()
    {
        if (ws != null)
        {
            Debug.Log("Закрытие соединения WebSocket...");
            ws.Close();
        }
    }
}