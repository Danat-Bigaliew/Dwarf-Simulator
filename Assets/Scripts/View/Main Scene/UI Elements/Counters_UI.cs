using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Counters_UI : MonoBehaviour
{
    [SerializeField] private BaseCanvasUI baseCanvasUI;
    [SerializeField] private ProgressBars_UI progressBarsUI;

    private RectTransform countersContainer;

    Dictionary<string, string> gameData = new Dictionary<string, string>();

    public float counterContainerPosY { get; private set; }

    public void CountersUI()
    {
        countersContainer = GetComponent<RectTransform>();

        UI();
    }

    private void UI()
    {
        float canvasWidth = baseCanvasUI.newCanvasWidth;
        float countersChildCount = countersContainer.childCount;

        counterContainerPosY = (progressBarsUI.progressBarContainterHeight * 1.15f) * -1f;
        float counterContainerHeight = Screen.height * 0.065f;

        float horizontalPaddingBettwinCounter = canvasWidth * 0.045f;
        float counterWidth = (canvasWidth - horizontalPaddingBettwinCounter * (countersChildCount + 1)) / countersChildCount;

        float counterLogoWidth = counterWidth * 0.3f;
        float counterLogoFontSize = counterLogoWidth * 1.25f;

        float paddingBettwinNumberCounter = counterWidth * 0.05f;

        float numberCounterPosX = paddingBettwinNumberCounter + counterLogoWidth;
        float numberCounterWidth = counterWidth * 0.6f;

        float tempCounterPosX = horizontalPaddingBettwinCounter;

        countersContainer.sizeDelta = new Vector2(countersContainer.sizeDelta.x, counterContainerHeight);
        countersContainer.anchoredPosition = new Vector2(countersContainer.anchoredPosition.x, counterContainerPosY);

        foreach (RectTransform childCounter in countersContainer)
        {
            childCounter.sizeDelta = new Vector2(counterWidth, childCounter.sizeDelta.y);
            childCounter.anchoredPosition = new Vector2(tempCounterPosX, childCounter.anchoredPosition.y);

            tempCounterPosX += horizontalPaddingBettwinCounter + counterWidth;

            RectTransform childCounterLogo = childCounter.GetChild(0).GetComponent<RectTransform>();
            TextMeshProUGUI childCounterLogoFontSize = childCounterLogo.GetComponent<TextMeshProUGUI>();

            childCounterLogo.sizeDelta = new Vector2(counterLogoWidth, childCounterLogo.sizeDelta.y);
            childCounterLogoFontSize.fontSize = counterLogoFontSize;

            RectTransform childCounter_numberCounter = childCounter.GetChild(1).GetComponent<RectTransform>();

            childCounter_numberCounter.sizeDelta = new Vector2(numberCounterWidth, childCounter_numberCounter.sizeDelta.y);
            childCounter_numberCounter.anchoredPosition = new Vector2(numberCounterPosX, childCounter_numberCounter.anchoredPosition.y);
        }
    }
}