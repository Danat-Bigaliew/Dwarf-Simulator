using UnityEngine;
using UnityEngine.UI;

public class SettingsScene : MonoBehaviour
{
    [SerializeField] private BaseCanvasUI baseCanvasUI;

    private GridLayoutGroup gridLayoutGroup;
    private Transform contentContainer;

    private float screenHeight;

    public void SetupSettingsUI()
    {
        contentContainer = GetComponent<Transform>();
        gridLayoutGroup = GetComponent<GridLayoutGroup>();

        MainUI();
    }

    private void MainUI()
    {
        float childContainerHeight = Screen.height * 0.16f;

        screenHeight = Screen.height;

        gridLayoutGroup.cellSize = new Vector2(gridLayoutGroup.cellSize.x, childContainerHeight);

        VolumeUI();
    }

    private void VolumeUI()
    {
        Transform volume = contentContainer.GetChild(0).GetComponent<Transform>();

        float volumeLeftPadding = baseCanvasUI.newCanvasWidth * 0.025f;
        float volumeWidth = baseCanvasUI.newCanvasWidth - (volumeLeftPadding * 2f);
        float musicTitleWidth = baseCanvasUI.newCanvasWidth * 0.349f;
        float musicSoundScaleWidth = baseCanvasUI.newCanvasWidth * 0.6f;
        float musicSoundScalePosX = musicTitleWidth;
        float musicSoundScaleButtonsPaddingPosX = musicSoundScaleWidth * 0.0765f;
        float musicSoundScaleMinusWidth = musicSoundScaleWidth * 0.135f;
        float musicVolumePosX = musicSoundScaleButtonsPaddingPosX * 2 + musicSoundScaleMinusWidth;
        float musicVolumeWidth = musicSoundScaleWidth * 0.385f;
        float musicSoundScaleButtonPlusPosX = musicSoundScaleButtonsPaddingPosX + musicVolumePosX + musicVolumeWidth;

        foreach (RectTransform childComponent in volume)
        {
            RectTransform childContainer = childComponent;
            RectTransform childContainerTitle = childContainer.GetChild(0).GetComponent<RectTransform>();
            RectTransform childContainerSoundScale = childComponent.GetChild(1).GetComponent<RectTransform>();
            RectTransform musicSoundScaleButtonsMinus = childContainerSoundScale.GetChild(0).GetComponent<RectTransform>();
            RectTransform musicSoundScaleButtonsPlus = childContainerSoundScale.GetChild(1).GetComponent<RectTransform>();
            RectTransform musicVolumeWidthRect = childContainerSoundScale.GetChild(2).GetComponent<RectTransform>();

            childContainer.sizeDelta = new Vector2(volumeWidth, childContainer.sizeDelta.y);
            childContainer.anchoredPosition = new Vector2(volumeLeftPadding, childContainer.anchoredPosition.y);

            childContainerTitle.sizeDelta = new Vector2(musicTitleWidth, childContainerTitle.sizeDelta.y);

            childContainerSoundScale.sizeDelta = new Vector2(musicSoundScaleWidth, childContainerSoundScale.sizeDelta.y);
            childContainerSoundScale.anchoredPosition = new Vector2(musicSoundScalePosX, childContainerSoundScale.anchoredPosition.y);

            musicSoundScaleButtonsMinus.sizeDelta = new Vector2(musicSoundScaleMinusWidth, musicSoundScaleMinusWidth * -1f);
            musicSoundScaleButtonsMinus.anchoredPosition = new Vector2(musicSoundScaleButtonsPaddingPosX, musicSoundScaleButtonsMinus.anchoredPosition.y);
            musicSoundScaleButtonsPlus.sizeDelta = new Vector2(musicSoundScaleMinusWidth, musicSoundScaleMinusWidth * -1f);
            musicSoundScaleButtonsPlus.anchoredPosition = new Vector2(musicSoundScaleButtonPlusPosX, musicSoundScaleButtonsPlus.anchoredPosition.y);

            musicVolumeWidthRect.sizeDelta = new Vector2(musicVolumeWidth, musicSoundScaleMinusWidth * -1f);
            musicVolumeWidthRect.anchoredPosition = new Vector2(musicVolumePosX, musicVolumeWidthRect.anchoredPosition.y);
        }
    }
}