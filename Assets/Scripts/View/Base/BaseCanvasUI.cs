using UnityEngine;

public class BaseCanvasUI : MonoBehaviour
{
    [SerializeField] private Texture2D referenceImage;

    private Transform gameCanvas;

    private Vector2 canvasRect;

    public float newCanvasWidth { get; private set; }

    public void BaseCanvas_UI()
    {
        gameCanvas = GetComponent<Transform>();

        BaseUI();
    }

    private void BaseUI()
    {
        canvasRect = new Vector2(Screen.width, Screen.height);

        float canvasProportions = canvasRect.x / canvasRect.y;
        float imageWidth = referenceImage.width;
        float imageHeight = referenceImage.height;

        newCanvasWidth = canvasRect.x * (canvasProportions / (imageWidth / imageHeight));

        foreach (Transform transformChild in gameCanvas.transform)
        {
            Canvas canvasChild = transformChild.GetComponent<Canvas>();
            RectTransform canvasRectTransform = canvasChild.GetComponent<RectTransform>();

            canvasChild.renderMode = RenderMode.WorldSpace;
            canvasRectTransform.sizeDelta = new Vector2(newCanvasWidth, canvasRectTransform.sizeDelta.y);
        }
    }
}