using System;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net.Http;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Zenject;

public class POSTRequest : MonoBehaviour, PostRequest
{
    [Inject] public PlayerDataService playerDataService;

    public Dictionary<string, string> forgeData { get; private set; } = new Dictionary<string, string>();
    public Dictionary<string, string> stockExchangeData { get; private set; } = new Dictionary<string, string>();
    public Dictionary<string, string> gameData { get; private set; } = new Dictionary<string, string>();

    public async void SignInLogIn(string login, string password, string nickname, string adress = "/")
    {
        login = CleanInput(login);
        password = CleanInput(password);
        nickname = CleanInput(nickname);

        using (HttpClient client = new HttpClient())
        {
            try
            {
                string url = "http://localhost:5000" + adress;

                var jsonData = new
                {
                    loginUser = login,
                    passwordUser = password,
                    nicknameUser = nickname
                };

                string jsonString = JsonConvert.SerializeObject(jsonData);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    Debug.Log($"responseBody : {responseBody}");

                    SuccessfulUserData(responseBody);
                }
                else
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    Debug.LogError(errorResponse);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Ошибка: " + ex.Message);
            }
        }
    }

    private void SuccessfulUserData(string responseBody)
    {
        GameObject sceneManager = GameObject.Find("SceneManager");
        Canvas bootstrapScene = sceneManager.GetComponent<Transform>().GetChild(0).GetComponent<Canvas>();

        bootstrapScene.sortingOrder = 10;

        ParsingRequest(responseBody);
        SceneManager.LoadScene("Main Scene");
    }

    private string CleanInput(string input)
    {
        return Regex.Replace(input, @"\p{Cf}+", string.Empty);
    }

    private void ParsingRequest(string response_Data)
    {
        try
        {
            JObject responseObject = JObject.Parse(response_Data);

            forgeData = new Dictionary<string, string>();
            stockExchangeData = new Dictionary<string, string>();
            gameData = new Dictionary<string, string>();

            foreach (var property in responseObject.Properties())
            {
                switch (property.Name)
                {
                    case "forge_data":
                        JObject forgeDataObject = (JObject)property.Value;

                        foreach (var forgeItem in forgeDataObject.Properties())
                        {
                            string id = forgeItem.Name;
                            JObject itemData = (JObject)forgeItem.Value;

                            foreach (var itemProperty in itemData.Properties())
                            {
                                string key = $"{itemProperty.Name} {id}";
                                string value = itemProperty.Value.ToString();

                                forgeData[key] = value;
                            }
                        }
                        break;

                    case "promotions_data":
                        JObject promotionsDataObject = (JObject)property.Value;

                        foreach (var promoItem in promotionsDataObject.Properties())
                        {
                            string id = promoItem.Name;
                            JObject promoData = (JObject)promoItem.Value;

                            foreach (var promoProperty in promoData.Properties())
                            {
                                string key = $"{promoProperty.Name} {id}";
                                string value = promoProperty.Value.ToString();

                                stockExchangeData[key] = value;
                            }
                        }
                        break;

                    case "game_data":
                        JObject gameDataObject = (JObject)property.Value;

                        foreach (var gameDataItem in gameDataObject.Properties())
                        {
                            string key = gameDataItem.Name;
                            string value = gameDataItem.Value.ToString();

                            gameData[key] = value;
                        }
                        break;

                    case "socket_name":
                        playerDataService.SetPlayerKey(property.Value.ToString());
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка при разборе JSON: {ex.Message}");
        }
    }

    public Dictionary<string, string> GetForgeData()
    {
        return forgeData;
    }

    public Dictionary<string, string> GetStockExchangeData()
    {
        return stockExchangeData;
    }

    public Dictionary<string, string> GetGameDataData()
    {
        return gameData;
    }
}