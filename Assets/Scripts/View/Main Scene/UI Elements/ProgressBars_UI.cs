using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProgressBars_UI : MonoBehaviour
{
    [Inject] PostRequest postRequest;

    [SerializeField] private BaseCanvasUI baseCanvasUI;

    [SerializeField] private float animationSpeed = 1.0f;

    private RectTransform progressBarContainter;
    private RectTransform happinessBar;
    private RectTransform strengthBar;
    private RectTransform eloquenceBar;

    public float progressBarContainterHeight { get; private set; }
    private float progressBarWidth;

    private Dictionary<string, string> gameData = new Dictionary<string, string>();

    public void ProgressBarsUI()
    {
        progressBarContainter = GetComponent<RectTransform>();

        UI();
        SetupGameData();
    }

    private void UI()
    {
        float newCanvasWidth = baseCanvasUI.newCanvasWidth;

        progressBarContainterHeight = Screen.height * 0.043f;
        progressBarWidth = newCanvasWidth * 0.285f;
        float paddingForBetweenBars = newCanvasWidth * 0.035f;

        float tempProgressBarsPosX = paddingForBetweenBars;

        progressBarContainter.sizeDelta = new Vector2(progressBarContainter.sizeDelta.x, progressBarContainterHeight);

        foreach (RectTransform childProgressBar in progressBarContainter)
        {
            childProgressBar.anchoredPosition = new Vector2(tempProgressBarsPosX, childProgressBar.anchoredPosition.y);
            childProgressBar.sizeDelta = new Vector2(progressBarWidth, childProgressBar.sizeDelta.y);

            tempProgressBarsPosX += progressBarWidth + paddingForBetweenBars;
        }

        happinessBar = progressBarContainter.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        strengthBar = progressBarContainter.GetChild(1).GetChild(0).GetComponent<RectTransform>();
        eloquenceBar = progressBarContainter.GetChild(2).GetChild(0).GetComponent<RectTransform>();
    }

    private void SetupGameData()
    {
        gameData = postRequest.GetGameDataData();

        float happiness = Convert.ToSingle(gameData["happiness"]);
        float strength = Convert.ToSingle(gameData["strength"]);
        float eloquence = Convert.ToSingle(gameData["eloquence"]);

        UpdateProgressBars(happiness, strength, eloquence);
    }

    public void UpdateProgressBars(float happinessValue, float strengthValue, float eloquenceValue)
    {
        float maxProgressBarWidth = progressBarWidth;

        float happinessStartValue = happinessBar.sizeDelta.x;
        float strengthStartValue = strengthBar.sizeDelta.x;
        float eloquenceStartValue = eloquenceBar.sizeDelta.x;

        float newHappinessValue = (happinessValue / 100) * progressBarWidth;
        float newStrengthValue = (strengthValue / 100) * progressBarWidth;
        float newEloquenceValue = (eloquenceValue / 100) * progressBarWidth;

        LeanTween.value(gameObject, happinessStartValue, newHappinessValue, animationSpeed)
            .setOnUpdate((float value) =>
            {
                Vector2 updatedSizeDelta = happinessBar.sizeDelta;
                updatedSizeDelta.x = value;
                happinessBar.sizeDelta = updatedSizeDelta;
            });

        LeanTween.value(gameObject, strengthStartValue, newStrengthValue, animationSpeed)
            .setOnUpdate((float value) =>
            {
                Vector2 updatedSizeDelta = strengthBar.sizeDelta;
                updatedSizeDelta.x = value;
                strengthBar.sizeDelta = updatedSizeDelta;
            });

        LeanTween.value(gameObject, eloquenceStartValue, newEloquenceValue, animationSpeed)
            .setOnUpdate((float value) =>
            {
                Vector2 updatedSizeDelta = eloquenceBar.sizeDelta;
                updatedSizeDelta.x = value;
                eloquenceBar.sizeDelta = updatedSizeDelta;
            });
    }
}