using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MoveSceneOnClick : MonoBehaviour
{
    [SerializeField] private ProgressBars_UI progressBarsUI;

    [Header("Canvas")]
    [SerializeField] private Canvas countersCanvas;
    [SerializeField] private Transform gameCanvasScene;

    [Header("Variables")]
    private Transform downMenu;

    [SerializeField] private RectTransform counters;

    [SerializeField] private List<RectTransform> scrollViewCanvasRectArray = new List<RectTransform>();
    [SerializeField] private List<CanvasGroup> canvasGroupArray = new List<CanvasGroup>();

    [SerializeField] private float timeAnimInSec;

    private Image downMenuBackground;

    private List<Button> downButtonArray = new List<Button>();
    private List<GameObject> gameCanvas = new List<GameObject>();
    private List<Canvas> gameSceneCanvas = new List<Canvas>();

    private CanvasGroup currentCanvasGroup;

    private const int orderLayerCurrentCanvas = 1;
    private const int orderLayerNotCurrentCanvas = 2;

    private int countCanvas;
    private int indexCurrentCanvas;
    private int clickedButtonIndex;

    private float posYActiveScene;
    private float posYNotActiveScene;
    private float countersHeight;

    private bool IsCounters = false;

    private ClicksController clicksController;

    [Inject]
    public void Construct(ClicksController ClicksController)
    {
        clicksController = ClicksController;
    }

    public void SetupButtons()
    {
        downMenuBackground = GetComponent<Image>();
        downMenu = GetComponent<Transform>();

        posYNotActiveScene = Screen.height * -1f;
        countCanvas = gameCanvasScene.childCount;

        countersHeight = counters.rect.height;

        for (int i = 0; i < downMenu.childCount; i++)
        {
            Button childButton = downMenu.GetChild(i).GetComponent<Button>();

            downButtonArray.Add(childButton);
        }

        foreach (Transform child in gameCanvasScene)
        {
            Canvas childCanvas = child.GetComponent<Canvas>();
            CanvasGroup canvasGroup = child.GetComponent<CanvasGroup>();
            GameObject childObject = child.gameObject;
            RectTransform childScrollView = child.GetChild(0).GetComponent<RectTransform>();

            gameCanvas.Add(childObject);
            canvasGroupArray.Add(canvasGroup);
            gameSceneCanvas.Add(childCanvas);
            scrollViewCanvasRectArray.Add(childScrollView);
        }

        currentCanvasGroup = gameSceneCanvas[1].GetComponent<CanvasGroup>();
        indexCurrentCanvas = gameSceneCanvas.IndexOf(currentCanvasGroup.GetComponent<Canvas>());

        gameSceneCanvas[indexCurrentCanvas].sortingOrder = orderLayerCurrentCanvas;

        DisableChildComponents(false);
        DisableChildComponents(true, indexCurrentCanvas);

        posYActiveScene = scrollViewCanvasRectArray[indexCurrentCanvas].offsetMax.y;
        downButtonArray[indexCurrentCanvas].interactable = false;

        for (int i = 0; i < downButtonArray.Count; i++)
        {
            int index = i;
            downButtonArray[i].onClick.AddListener(() => OnButtonClick(downButtonArray[index]));
        }
    }

    private void OnButtonClick(Button clickedButton)
    {
        currentCanvasGroup.interactable = false;
        currentCanvasGroup.blocksRaycasts = false;

        clickedButtonIndex = downButtonArray.IndexOf(clickedButton);

        downButtonArray[clickedButtonIndex].interactable = false;

        clicksController.ButtonClickAudio();

        DisableChildComponents(true, clickedButtonIndex);
        MovePressedScene(clickedButtonIndex, scrollViewCanvasRectArray[clickedButtonIndex], counters);
    }

    private void MovePressedScene(int clickedButtonIndex, RectTransform pressedCanvas, RectTransform counters)
    {
        Color colorBackground = downMenuBackground.color;

        float pressedCanvasInitialOffsetMaxY = pressedCanvas.offsetMax.y;
        float countersInitialOffsetMaxY = counters.offsetMax.y;

        float pressedCanvasTargetOffsetMaxY = progressBarsUI.progressBarContainterHeight * -1f;
        float countersTargetOffsetMaxY = 0f;

        switch (IsCounters)
        {
            case false:
                countersTargetOffsetMaxY = counters.rect.height;
                break;
            case true:
                countersTargetOffsetMaxY = (progressBarsUI.progressBarContainterHeight + 5) * -1f;
                break;
        }

        float initialAlpha = colorBackground.a;
        float targetAlpha = clickedButtonIndex == 1 ? 0f : 1f;

        LeanTween.value(initialAlpha, targetAlpha, timeAnimInSec)
                 .setEase(LeanTweenType.easeInOutQuad)
                 .setOnUpdate((float alphaValue) =>
                 {
                     colorBackground.a = alphaValue;
                     downMenuBackground.color = colorBackground;
                 });

        LeanTween.value(pressedCanvasInitialOffsetMaxY, pressedCanvasTargetOffsetMaxY, timeAnimInSec)
                 .setEase(LeanTweenType.easeInOutQuad)
                 .setOnUpdate((float value) =>
                 {
                     Vector2 offsetMax = pressedCanvas.offsetMax;

                     offsetMax.y = value;
                     pressedCanvas.offsetMax = offsetMax;

                     float alphaValue = Mathf.Lerp(initialAlpha, targetAlpha, value / pressedCanvasTargetOffsetMaxY);
                 });

        LeanTween.value(countersInitialOffsetMaxY, countersTargetOffsetMaxY, timeAnimInSec)
                 .setEase(LeanTweenType.easeInOutQuad)
                 .setOnUpdate((float value) =>
                 {
                     counters.sizeDelta = new Vector2(counters.sizeDelta.x, countersHeight);
                     Vector2 offsetMax = counters.offsetMax;

                     offsetMax.y = value;
                     counters.offsetMax = offsetMax;

                     float alphaValue = Mathf.Lerp(initialAlpha, targetAlpha, value / countersTargetOffsetMaxY);
                 })

                 .setOnComplete(() =>
                 {
                     IsCounters = !IsCounters;

                     canvasGroupArray[clickedButtonIndex].interactable = true;
                     canvasGroupArray[clickedButtonIndex].blocksRaycasts = true;

                     DisableChildComponents(false, indexCurrentCanvas);
                     MoveCurrentScene(clickedButtonIndex);
                 });
    }

    private void DisableChildComponents(bool point, int indexCanvas = -1)
    {
        switch (indexCanvas)
        {
            case -1:
                for (int i = 0; i < gameCanvas.Count; i++)
                {
                    gameCanvas[i].SetActive(point);
                }
                break;
            default:
                gameCanvas[indexCanvas].SetActive(point);
                break;
        }
    }

    private void MoveCurrentScene(int tempIndexCanvas)
    {
        Vector2 offsetMax = scrollViewCanvasRectArray[indexCurrentCanvas].offsetMax;
        offsetMax.y = posYNotActiveScene;

        float posYCountersScene = 0;

        switch (tempIndexCanvas)
        {
            case 1:
                posYCountersScene = (progressBarsUI.progressBarContainterHeight + 5) * -1f;
                counters.anchoredPosition = new Vector2(counters.anchoredPosition.x, posYCountersScene);
                break;
            default:
                posYCountersScene = Screen.height * -1f;
                counters.anchoredPosition = new Vector2(counters.anchoredPosition.x, posYCountersScene);
                break;
        }

        scrollViewCanvasRectArray[indexCurrentCanvas].offsetMax = offsetMax;

        gameSceneCanvas[indexCurrentCanvas].sortingOrder = orderLayerNotCurrentCanvas;

        downButtonArray[indexCurrentCanvas].interactable = true;
        currentCanvasGroup = gameSceneCanvas[clickedButtonIndex].GetComponent<CanvasGroup>();
        indexCurrentCanvas = gameSceneCanvas.IndexOf(currentCanvasGroup.GetComponent<Canvas>());

        gameSceneCanvas[indexCurrentCanvas].sortingOrder = orderLayerCurrentCanvas;
    }
}