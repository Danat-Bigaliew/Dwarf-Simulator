using UnityEngine;
using UnityEngine.UI;

public class ShaftSceneUI : MonoBehaviour
{
    [SerializeField] private BaseCanvasUI baseCanvasUI;
    [SerializeField] private ProgressBars_UI progressBarsUI;
    [SerializeField] private DownMenu_UI downMenuUI;

    private GridLayoutGroup gridLayoutGroup;

    public void UI_Item_Main()
    {
        gridLayoutGroup = GetComponent<GridLayoutGroup>();

        MainUI();
    }

    private void MainUI()
    {
        float newHeight = Screen.height - progressBarsUI.progressBarContainterHeight;

        gridLayoutGroup.cellSize = new Vector2(baseCanvasUI.newCanvasWidth, newHeight);
    }
}