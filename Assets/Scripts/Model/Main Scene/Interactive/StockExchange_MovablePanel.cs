using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StockExchange_MovablePanel : MonoBehaviour
{
    [SerializeField] private float speedAnimation;

    private StockExchangeSceneUI stockExchangeSceneUI;
    private GridLayoutGroup gridLayoutGroup;

    private float itemHeight;
    private List<float> itemPosY = new List<float>();

    private RectTransform content;
    private List<RectTransform> movablePanelArray = new List<RectTransform>();

    private List<Button> buttonsArray = new List<Button>();

    private bool isItemPressed = false;

    public void SetupVariables(float itemSizeDelta)
    {
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
        stockExchangeSceneUI = GetComponent<StockExchangeSceneUI>();
        content = GetComponent<RectTransform>();

        itemHeight = itemSizeDelta;

        SetupButtons();
    }

    private void SetupButtons()
    {
        for (int i = 0; i < content.childCount; i++)
        {
            RectTransform childItem = content.GetChild(i).GetComponent<RectTransform>();
            RectTransform itemMovablePanel = childItem.GetChild(2).GetComponent<RectTransform>();

            Button itemButton = childItem.GetComponent<Button>();

            int index = i;

            itemPosY.Add(childItem.anchoredPosition.y);
            buttonsArray.Add(itemButton);
            movablePanelArray.Add(itemMovablePanel);

            itemButton.onClick.AddListener(() => ClickOnItemButton(index));
        }
    }

    private void ClickOnItemButton(int index)
    {
        RectTransform panel = movablePanelArray[index];

        int pressedItem = index + 1;

        float movableTableHeight = stockExchangeSceneUI.activeMenuHeight;
        float targetHeight = movableTableHeight;
        float panelHeight = stockExchangeSceneUI.activeMenuHeight;

        switch (isItemPressed)
        {
            case false:
                float signForPanelHeight = -1f;

                StartCoroutine(AnimatePanelHeight(panel, speedAnimation, targetHeight));
                StartCoroutine(AnimateItems(content, speedAnimation, index, panelHeight, signForPanelHeight));

                isItemPressed = PreparationAnimation(index, isItemPressed);
                break;
            case true:
                targetHeight = 0;

                StartCoroutine(AnimatePanelHeight(panel, speedAnimation));
                StartCoroutine(AnimateItems(content, speedAnimation, index, panelHeight));

                isItemPressed = PreparationAnimation(index, isItemPressed);
                break;
        }

        if (content.childCount == pressedItem && isItemPressed == true)
            gridLayoutGroup.constraintCount = content.childCount + 2;
        else if (content.childCount == pressedItem && isItemPressed == false)
            gridLayoutGroup.constraintCount = content.childCount;
    }

    private bool PreparationAnimation(float index, bool isItemPressed)
    {
        for (int i = 0; i < buttonsArray.Count; i++)
        {
            if (i != index)
            {
                buttonsArray[i].interactable = isItemPressed;
            }
        }

        return isItemPressed = !isItemPressed;
    }

    IEnumerator AnimatePanelHeight(RectTransform panel, float speedAnimation, float targetHeight = 0)
    {
        LeanTween.value(panel.gameObject, panel.sizeDelta.y, targetHeight, speedAnimation)
             .setOnUpdate((float newHeight) =>
             {
                 panel.sizeDelta = new Vector2(panel.sizeDelta.x, newHeight);
             })
             .setEase(LeanTweenType.easeInOutQuad);

        yield return new WaitForSeconds(speedAnimation);
    }

    IEnumerator AnimateItems(RectTransform content, float speedAnimation, int index, float panelHeight, float signForPanelHeight = 1f)
    {
        for (int i = index + 1; i < content.childCount; i++)
        {
            RectTransform nextItem = content.GetChild(i).GetComponent<RectTransform>();

            float nextTargetPosY = panelHeight * signForPanelHeight + nextItem.anchoredPosition.y;

            LeanTween.value(nextItem.gameObject, nextItem.anchoredPosition.y, nextTargetPosY, speedAnimation)
                 .setOnUpdate((float newPosY) =>
                 {
                     nextItem.anchoredPosition = new Vector2(nextItem.anchoredPosition.x, newPosY);
                 })
                 .setEase(LeanTweenType.easeInOutQuad);
        }

        yield return new WaitForSeconds(speedAnimation);
    }
}