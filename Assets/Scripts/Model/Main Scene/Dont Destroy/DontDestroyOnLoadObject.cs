using UnityEngine;

public class DontDestroyOnLoadObject : MonoBehaviour
{
    private BaseCanvasUI baseCanvasUI;
    private Canvas bootstrapSceneCanvas;

    private Transform sceneManager;

    public void SetupDontDestroyOnLoad()
    {
        sceneManager = GetComponent<Transform>();
        baseCanvasUI = GetComponent<BaseCanvasUI>();
        bootstrapSceneCanvas = sceneManager.GetChild(0).GetComponent<Canvas>();

        baseCanvasUI.BaseCanvas_UI();

        bootstrapSceneCanvas.sortingOrder = -1;


        if (FindDuplicateInstance())
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private bool FindDuplicateInstance()
    {
        DontDestroyOnLoadObject[] instances = Object.FindObjectsByType<DontDestroyOnLoadObject>(FindObjectsSortMode.None);
        foreach (DontDestroyOnLoadObject instance in instances)
        {
            if (instance != this)
            {
                return true;
            }
        }
        return false;
    }
}