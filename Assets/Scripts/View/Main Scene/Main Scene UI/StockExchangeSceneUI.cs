using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;

public class StockExchangeSceneUI : MonoBehaviour
{
    [Inject] PostRequest postRequest;

    [SerializeField] private BaseCanvasUI baseCanvasUI;
    [SerializeField] private GameObject stockExchangePrefab;

    private StockExchange_MovablePanel stockExchangeMovablePanel;

    private Dictionary<string, string> stockExchangeData = new Dictionary<string, string>();

    private RectTransform content;
    private RectTransform item;

    private GridLayoutGroup gridLayoutGroup;

    public float activeMenuHeight { get; private set; }
    public float itemHeight { get; private set; }

    private PlayerDataOnSession playerDataOnSession;

    [Inject]
    public void Construct(PlayerDataOnSession PlayerDataOnSession)
    {
        playerDataOnSession = PlayerDataOnSession;
    }

    public void UI_Item_StockExchange()
    {
        stockExchangeMovablePanel = GetComponent<StockExchange_MovablePanel>();
        content = GetComponent<RectTransform>();
        item = content.GetChild(0).GetComponent<RectTransform>();

        gridLayoutGroup = GetComponent<GridLayoutGroup>();

        ItemUI();
        GetDataStockExchange();
    }

    private void ItemUI()
    {
        float newCanvasWidth = baseCanvasUI.newCanvasWidth;

        itemHeight = Screen.height * 0.16f;

        float horizontalPadding = newCanvasWidth * 0.045f * -1f;

        float itemLogoWidth = newCanvasWidth * 0.185f;
        float itemTitleDescriptionWidth = newCanvasWidth * 0.6f;
        float itemTitleDescriptionPosX = itemLogoWidth * -1f + (horizontalPadding * 2f);

        float itemTitleFontSize = itemTitleDescriptionWidth * 0.115f;
        float itemDescriptionFontSize = itemTitleDescriptionWidth * 0.066f;

        gridLayoutGroup.cellSize = new Vector2(newCanvasWidth, itemHeight);

        float referensLogoWidth = 80f;

        item = stockExchangePrefab.GetComponent<RectTransform>();

        RectTransform itemLogo = item.GetChild(0).GetComponent<RectTransform>();
        RectTransform itemTitle = item.GetChild(1).GetComponent<RectTransform>();

        TextMeshProUGUI itemFontSize = itemTitle.GetComponent<TextMeshProUGUI>();

        itemLogo.sizeDelta = new Vector2(itemLogoWidth, (referensLogoWidth - itemLogoWidth) * -1f);
        itemLogo.anchoredPosition = new Vector2(horizontalPadding, itemLogo.anchoredPosition.y);

        itemTitle.sizeDelta = new Vector2(itemTitleDescriptionWidth, itemTitle.sizeDelta.y);
        itemTitle.anchoredPosition = new Vector2(itemTitleDescriptionPosX, itemTitle.anchoredPosition.y);

        itemFontSize.fontSize = itemTitleFontSize;
        Item_activeMenu(itemHeight, newCanvasWidth);
    }

    private void Item_activeMenu(float itemContainerHeight, float newCanvasWidth)
    {
        activeMenuHeight = itemContainerHeight * 2f;

        float descriptionHorizontalPadding = activeMenuHeight * 0.07f * -1f;
        float descriptionVerticalPadding = activeMenuHeight * 0.1f;
        float infoItemsHorizontalPadding = newCanvasWidth * 0.0465f;

        float descriptionHeight = activeMenuHeight / 2f;
        float ifoItemsHeight = activeMenuHeight - descriptionVerticalPadding - descriptionHeight;
        float actionRateHeight = newCanvasWidth * 0.14f;
        float buttonsPriceQuantityHeight = newCanvasWidth * 0.372f;

        float descriptionPosY = descriptionVerticalPadding / 2f * -1f;
        float infoItemsPosY = (activeMenuHeight - (descriptionPosY + descriptionHeight)) * -1f;
        float buttonsPosX = infoItemsHorizontalPadding + actionRateHeight;
        float priceQuantityPosX = infoItemsHorizontalPadding / 2f + buttonsPosX + buttonsPriceQuantityHeight;

        RectTransform itemActiveMenu = item.GetChild(2).GetComponent<RectTransform>();
        RectTransform itemDescriptionRect = itemActiveMenu.GetChild(0).GetComponent<RectTransform>();
        RectTransform itemInfoItemsRect = itemActiveMenu.GetChild(1).GetComponent<RectTransform>();

        RectTransform ifoItemsActionRate = itemInfoItemsRect.GetChild(0).GetComponent<RectTransform>();
        RectTransform infoItemsButtons = itemInfoItemsRect.GetChild(1).GetComponent<RectTransform>();
        RectTransform infoItemsPriceAndQuantity = itemInfoItemsRect.GetChild(2).GetComponent<RectTransform>();

        TextMeshProUGUI itemDescriptionTMP = itemDescriptionRect.GetComponent<TextMeshProUGUI>();

        itemActiveMenu.sizeDelta = new Vector2(itemActiveMenu.sizeDelta.x, itemActiveMenu.sizeDelta.y);
        itemActiveMenu.anchoredPosition = new Vector2(itemActiveMenu.anchoredPosition.x, itemContainerHeight * -1f);

        itemDescriptionRect.sizeDelta = new Vector2(descriptionHorizontalPadding, descriptionHeight);
        itemDescriptionRect.anchoredPosition = new Vector2(itemDescriptionRect.anchoredPosition.x, descriptionPosY);

        itemInfoItemsRect.sizeDelta = new Vector2(descriptionHorizontalPadding, ifoItemsHeight);
        itemInfoItemsRect.anchoredPosition = new Vector2(itemInfoItemsRect.anchoredPosition.x, infoItemsPosY);

        itemInfoItemsRect.sizeDelta = new Vector2(descriptionHorizontalPadding, ifoItemsHeight);

        ifoItemsActionRate.sizeDelta = new Vector2(actionRateHeight, ifoItemsActionRate.sizeDelta.y);

        infoItemsButtons.sizeDelta = new Vector2(buttonsPriceQuantityHeight, infoItemsButtons.sizeDelta.y);
        infoItemsButtons.anchoredPosition = new Vector2(buttonsPosX, infoItemsButtons.anchoredPosition.y);

        infoItemsPriceAndQuantity.sizeDelta = new Vector2(buttonsPriceQuantityHeight, infoItemsPriceAndQuantity.sizeDelta.y);
        infoItemsPriceAndQuantity.anchoredPosition = new Vector2(priceQuantityPosX, infoItemsPriceAndQuantity.anchoredPosition.y);
    }

    private void GetDataStockExchange()
    {
        stockExchangeData = postRequest.GetStockExchangeData();

        gridLayoutGroup.constraintCount = stockExchangeData.Count / 5;

        SetupStockExchangeData(item.transform, 0);

        for (int i = 1; i < stockExchangeData.Count / 5; ++i)
        {
            GameObject newItem = Instantiate(item.gameObject, content.transform);
            Transform itemTransform = newItem.GetComponent<Transform>();

            SetupStockExchangeData(itemTransform, i);
        }

        stockExchangeMovablePanel.SetupVariables(item.sizeDelta.y);
    }

    private void SetupStockExchangeData(Transform item, int count)
    {
        Transform activeMenu = item.GetChild(2).GetComponent<Transform>();
        Transform infoItems = activeMenu.GetChild(1).GetComponent<Transform>();
        Transform priceAndQuantity = infoItems.GetChild(2).GetComponent<Transform>();

        Transform actionRate = infoItems.GetChild(0).GetComponent<Transform>();

        GameObject stockRise = actionRate.GetChild(0).gameObject;
        GameObject stockFall = actionRate.GetChild(1).gameObject;

        TextMeshProUGUI title = item.GetChild(1).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI description = activeMenu.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI priceCounter = priceAndQuantity.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI quantity = priceAndQuantity.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();

        ++count;

        title.text = stockExchangeData[$"title {count}"];
        description.text = stockExchangeData[$"description {count}"];
        quantity.text = stockExchangeData[$"quantity_promotion {count}"];
        priceCounter.text = $"{stockExchangeData[$"price {count}"]}G";

        switch (stockExchangeData[$"price_sign {count}"])
        {
            case "+":
                stockRise.SetActive(true);
                stockFall.SetActive(false);
                break;
            case "-":
                stockRise.SetActive(false);
                stockFall.SetActive(true);
                break;
        }
    }

    public void UpdateStockExchangeData(int indexItem, int payButtonText)
    {
        Transform item = content.transform.GetChild(indexItem - 1).GetComponent<Transform>();
        Transform activeMenu = item.GetChild(2).GetComponent<Transform>();
        Transform infoItems = activeMenu.GetChild(1).GetComponent<Transform>();
        Transform priceAndQuantity = infoItems.GetChild(2).GetComponent<Transform>();

        TextMeshProUGUI quantity = priceAndQuantity.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();

        quantity.text = $"X: {payButtonText}";
    }
}