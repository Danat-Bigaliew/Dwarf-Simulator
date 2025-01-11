using UnityEngine;
using TMPro;

public class DownMenu_UI : MonoBehaviour
{
    [SerializeField] private BaseCanvasUI baseCanvasUI;

    private RectTransform downMenu;

    public float downMenuContainerHeight { get; private set; }

    public void DownMenuUI()
    {
        downMenu = GetComponent<RectTransform>();

        UI();
    }

    private void UI()
    {
        downMenuContainerHeight = Screen.height * 0.091f;

        float downMenuChildHorizontalPadding = baseCanvasUI.newCanvasWidth * 0.04f;

        float downMenuChildWidth = baseCanvasUI.newCanvasWidth * 0.2f;
        float downMenuChildFontSize = downMenuChildWidth * 0.2f;

        float tempHorizontallPadding = downMenuChildHorizontalPadding;

        downMenu.sizeDelta = new Vector2(downMenu.sizeDelta.x, downMenuContainerHeight);

        foreach (RectTransform downMenuChild in downMenu)
        {
            downMenuChild.sizeDelta = new Vector2(downMenuChildWidth, downMenuChild.sizeDelta.y);
            downMenuChild.anchoredPosition = new Vector2(tempHorizontallPadding, downMenuChild.anchoredPosition.y);

            tempHorizontallPadding += downMenuChildHorizontalPadding + downMenuChildWidth;

            TextMeshProUGUI downMenuFontSize = downMenuChild.GetChild(0).GetComponent<TextMeshProUGUI>();

            downMenuFontSize.fontSize = downMenuChildFontSize;
        }
    }
}
