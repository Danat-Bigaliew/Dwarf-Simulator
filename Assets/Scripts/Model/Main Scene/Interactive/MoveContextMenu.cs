using System.Collections;
using TMPro;
using UnityEngine;

public class MoveContextMenu : MonoBehaviour
{
    [SerializeField] private BaseCanvasUI baseCanvasUI;
    private ContextMenu contextMenu;

    private RectTransform content;
    private TextMeshProUGUI messageText;
    private float initialSize;
    private float targetSize;
    private float animationDuration = 0.2f;
    private float timeContextMenu = 0.6f;

    public void SetupMoveContextMenu()
    {
        contextMenu = GetComponent<ContextMenu>();

        content = GetComponent<RectTransform>();
        messageText = content.GetChild(0).GetComponent<TextMeshProUGUI>();

        initialSize = 0;
        targetSize = contextMenu.contextMenuHeight;
    }

    public IEnumerator AnimationContent(string message)
    {
        messageText.text = message;

        LeanTween.value(gameObject, content.sizeDelta.y, targetSize, animationDuration)
                .setOnUpdate((float newSize) =>
                {
                    content.sizeDelta = new Vector2(content.sizeDelta.x, newSize);
                })
                .setEase(LeanTweenType.easeInOutQuad);

        yield return new WaitForSeconds(timeContextMenu);

        LeanTween.value(gameObject, content.sizeDelta.y, initialSize, animationDuration)
                .setOnUpdate((float newSize) =>
                {
                    content.sizeDelta = new Vector2(content.sizeDelta.x, newSize);
                })
                .setEase(LeanTweenType.easeInOutQuad);
    }
}