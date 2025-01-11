using UnityEngine;

public class SignIn_LogIn_UI : MonoBehaviour
{
    [SerializeField] BaseCanvasUI baseCanvasUI;

    private Transform content;

    private float titleHeight;

    private float inputFieldPosY;
    private float inputFieldWidth;
    private float inputFieldHeight;
    private float inputFieldVerticalPadding;

    public void LogRegUI()
    {
        content = GetComponent<Transform>();

        Title();
        InputFields();
        Buttoms();
    }

    private void Title()
    {
        Transform title = content.transform.GetChild(0);
        RectTransform titleRect = title.GetComponent<RectTransform>();

        float titleWidth = baseCanvasUI.newCanvasWidth * 0.465f;
        float titlePosY = Screen.height * 0.01f * -1f;
        titleHeight = baseCanvasUI.newCanvasWidth * 0.35f;

        titleRect.anchoredPosition = new Vector2(0f, titlePosY);
        titleRect.sizeDelta = new Vector2(titleWidth, titleHeight);
    }

    private void InputFields()
    {
        Transform inputField = content.transform.GetChild(1);
        RectTransform inputFieldRect = inputField.GetComponent<RectTransform>();

        float inputFieldHorizontalPadding = baseCanvasUI.newCanvasWidth * 0.1f;
        float itemHeight = Screen.height * 0.11f;

        inputFieldVerticalPadding = titleHeight * 0.2f;
        inputFieldPosY = (titleHeight + inputFieldVerticalPadding) * -1f;
        inputFieldWidth = baseCanvasUI.newCanvasWidth - (inputFieldHorizontalPadding * 2f);
        inputFieldHeight = itemHeight * 3 + inputFieldVerticalPadding * 2f;

        float paddingBetweenInputFieldItem = inputFieldVerticalPadding / 4f;
        float textPointWidth = inputFieldWidth * 0.29f;
        float itemInputFieldPosX = textPointWidth + paddingBetweenInputFieldItem;
        float itemInputFieldWidth = inputFieldWidth - itemInputFieldPosX;

        inputFieldRect.sizeDelta = new Vector2(inputFieldWidth, inputFieldHeight);
        inputFieldRect.anchoredPosition = new Vector2(0f, inputFieldPosY);

        foreach (Transform childInputField in inputField)
        {
            RectTransform textforPointRectTransform = childInputField.transform.GetChild(0).GetComponent<RectTransform>();
            RectTransform itemInputFieldRectTransform = childInputField.transform.GetChild(1).GetComponent<RectTransform>();

            textforPointRectTransform.sizeDelta = new Vector2(textPointWidth, textforPointRectTransform.sizeDelta.y);
            itemInputFieldRectTransform.anchoredPosition = new Vector2(itemInputFieldPosX, itemInputFieldRectTransform.anchoredPosition.y);
            itemInputFieldRectTransform.sizeDelta = new Vector2(itemInputFieldWidth, itemInputFieldRectTransform.sizeDelta.y);
        }
    }

    private void Buttoms()
    {
        Transform buttomsTransform = content.transform.GetChild(2);
        RectTransform buttoms = buttomsTransform.GetComponent<RectTransform>();

        float buttonsPosY = ((inputFieldPosY * -1f) + inputFieldHeight + (inputFieldVerticalPadding * 2f)) * -1f;
        float buttonsWidth = inputFieldWidth;
        float buttonsHeight = titleHeight * 2f;
        float paddingBetweenItemButtons = inputFieldVerticalPadding * 2f;
        float itemButtonHeight = (buttonsHeight - paddingBetweenItemButtons) / 2f;
        float tempPaddingBetweenButtons = (itemButtonHeight + paddingBetweenItemButtons) * -1f;

        buttoms.anchoredPosition = new Vector2(0f, buttonsPosY);
        buttoms.sizeDelta = new Vector2(buttonsWidth, buttonsHeight);

        foreach (Transform childButton in buttomsTransform.transform)
        {
            tempPaddingBetweenButtons += itemButtonHeight + paddingBetweenItemButtons;

            RectTransform childButtonRectTransform = childButton.GetComponent<RectTransform>();

            childButtonRectTransform.sizeDelta = new Vector2(childButtonRectTransform.sizeDelta.x, itemButtonHeight);
            childButtonRectTransform.anchoredPosition = new Vector2(0f, tempPaddingBetweenButtons * -1f);
        }
    }
}