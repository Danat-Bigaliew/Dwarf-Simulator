using TMPro;
using UnityEngine;

public class ContextMenu : MonoBehaviour
{
    [SerializeField] private BaseCanvasUI baseCanvasUI;
    private RectTransform content;
    private TextMeshProUGUI contentText;

    public float contextMenuHeight { get; private set; }

    public void SetupContextMenu()
    {
        content = GetComponent<RectTransform>();
        contentText = content.GetChild(0).GetComponent<TextMeshProUGUI>();

        ContextMenuUI();
    }

    private void ContextMenuUI()
    {
        float contextMenuPosY = Screen.height * 0.7f * -1f;
        float contextMenuWidth = baseCanvasUI.newCanvasWidth * 0.2f * -1f;
        float contextMenuSizeText = contextMenuWidth * -1f * 0.35f;
        contextMenuHeight = Screen.height * 0.1f;

        content.anchoredPosition = new Vector2(content.anchoredPosition.x, contextMenuPosY);
        content.sizeDelta = new Vector2(contextMenuWidth, 0);
        contentText.fontSize = contextMenuSizeText;
    }
}