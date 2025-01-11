using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;
using System.Collections.Generic;

public class MarketSceneUI : MonoBehaviour
{
    [Inject] PostRequest postRequest;

    [SerializeField] private BaseCanvasUI baseCanvasUI;

    [SerializeField] private GameObject itemPrefab;

    Dictionary<string, string> forgeData = new Dictionary<string, string>();

    private RectTransform content;
    private RectTransform item;

    private GridLayoutGroup gridLayoutGroup;

    private PlayerDataOnSession playerDataOnSession;

    [Inject]
    public void Construct(PlayerDataOnSession PlayerDataOnSession)
    {
        playerDataOnSession = PlayerDataOnSession;
    }

    public void UI_Item_Market()
    {
        content = GetComponent<RectTransform>();

        gridLayoutGroup = GetComponent<GridLayoutGroup>();

        ItemUI();
        GetDataForge();
    }

    private void ItemUI()
    {
        float itemHeight = Screen.height * 0.14f - (Screen.width - baseCanvasUI.newCanvasWidth) / 2;

        float paddingBettwinComponentsItems = gridLayoutGroup.cellSize.x * 0.02f;
        float logoWidth = paddingBettwinComponentsItems * 8f;

        float titlePosX = paddingBettwinComponentsItems * 2f + logoWidth;
        float titledescriptionWidth = baseCanvasUI.newCanvasWidth * 0.45f;

        float descriptionPosX = titlePosX;

        float payButtonPriceCounterPosX = descriptionPosX + titledescriptionWidth + paddingBettwinComponentsItems;
        float payButtonWidth = baseCanvasUI.newCanvasWidth - (payButtonPriceCounterPosX + paddingBettwinComponentsItems);

        float priceCounterWidth = baseCanvasUI.newCanvasWidth - (payButtonPriceCounterPosX + paddingBettwinComponentsItems);

        item = itemPrefab.GetComponent<RectTransform>();

        RectTransform itemLogo = item.GetChild(0).GetComponent<RectTransform>();
        RectTransform itemTitle = item.GetChild(1).GetComponent<RectTransform>();
        RectTransform itemDescription = item.GetChild(2).GetComponent<RectTransform>();
        RectTransform itemCounter = item.GetChild(3).GetComponent<RectTransform>();
        RectTransform itemPayButton = item.GetChild(4).GetComponent<RectTransform>();

        gridLayoutGroup.cellSize = new Vector2(baseCanvasUI.newCanvasWidth, itemHeight);

        itemLogo.sizeDelta = new Vector2(logoWidth, itemLogo.sizeDelta.y);
        itemLogo.anchoredPosition = new Vector2(paddingBettwinComponentsItems, itemLogo.anchoredPosition.y);

        itemTitle.sizeDelta = new Vector2(titledescriptionWidth, itemTitle.sizeDelta.y);
        itemTitle.anchoredPosition = new Vector2(titlePosX, itemTitle.anchoredPosition.y);

        itemDescription.sizeDelta = new Vector2(titledescriptionWidth, itemDescription.sizeDelta.y);
        itemDescription.anchoredPosition = new Vector2(descriptionPosX, itemDescription.anchoredPosition.y);

        itemCounter.sizeDelta = new Vector2(priceCounterWidth, itemCounter.sizeDelta.y);
        itemCounter.anchoredPosition = new Vector2(payButtonPriceCounterPosX, itemCounter.anchoredPosition.y);

        itemPayButton.anchoredPosition = new Vector2(payButtonPriceCounterPosX, itemPayButton.anchoredPosition.y);
        itemPayButton.sizeDelta = new Vector2(payButtonWidth, itemPayButton.sizeDelta.y);
    }

    private void GetDataForge()
    {
        forgeData = postRequest.GetForgeData();

        gridLayoutGroup.constraintCount = forgeData.Count / 4;

        SetupForgeData(item.transform, 1);

        for (int i = 2; i < (forgeData.Count / 4 + 1); ++i)
        {
            GameObject newItem = Instantiate(item.gameObject, content.transform);

            SetupForgeData(newItem.transform, i);
        }
    }

    private void SetupForgeData(Transform item, int count)
    {
        TextMeshProUGUI itemTitle = item.GetChild(1).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI itemDescription = item.GetChild(2).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI itemPriceCounter = item.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI itemPayButton = item.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>();

        itemTitle.text = forgeData[$"title {count}"];
        itemDescription.text = forgeData[$"description {count}"];
        itemPayButton.text = forgeData[$"level {count}"];
        itemPriceCounter.text = $"{forgeData[$"price_for_ui {count}"]} G";
    }

    public void UpdateForgeData(int indexItem, string payButtonText, string price)
    {
        Transform item = content.transform.GetChild(indexItem - 1).GetComponent<Transform>();

        TextMeshProUGUI itemPriceCounter = item.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI itemPayButton = item.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>();

        switch (payButtonText)
        {
            case "":// MAX
                itemPriceCounter.text = "";
                itemPayButton.text = payButtonText;
                break;
            default:// < MAX
                itemPriceCounter.text = $"{price} G";
                itemPayButton.text = payButtonText;
                break;
        }
    }
}