using UnityEngine;

public class Setup_MainScene : MonoBehaviour
{
    [Header("Base")]
    [SerializeField] private BaseCanvasUI baseCanvasUI;
    [SerializeField] private BaseCanvasUI baseCanvasUIElements;

    [Header("View")]
    [SerializeField] private ShaftSceneUI shaftSceneUI;
    [SerializeField] private MarketSceneUI marketSceneUI;
    [SerializeField] private StockExchangeSceneUI stockExchangeSceneUI;
    [SerializeField] private SettingsScene settingsScene;
    [SerializeField] private ProgressBars_UI progressBarsUI;
    [SerializeField] private Counters_UI countersUI;
    [SerializeField] private DownMenu_UI downMenuUI;
    [SerializeField] private ContextMenu contextMenu;


    [Header("Model")]
    [SerializeField] private MoveSceneOnClick moveSceneOnClick;
    [SerializeField] private MoveContextMenu moveContextMenu;
    [SerializeField] private BuyItems_forge buyItemsForge;
    [SerializeField] private BuyItems_StockExchange buyItemsStockExchange;
    [SerializeField] private Counters_Animation countersAnimation;

    [SerializeField] private VolumeController volumeController;

    [Header("Service")]
    private WebSocketConnect webSocketConnect;

    private void Awake()
    {
        webSocketConnect = GetComponent<WebSocketConnect>();

        View();
        Model();
        Service();

        OffBootstrapScene();
    }

    private void View()
    {
        baseCanvasUI.BaseCanvas_UI();
        baseCanvasUIElements.BaseCanvas_UI();
        marketSceneUI.UI_Item_Market();
        stockExchangeSceneUI.UI_Item_StockExchange();
        settingsScene.SetupSettingsUI();

        progressBarsUI.ProgressBarsUI();
        countersUI.CountersUI();
        downMenuUI.DownMenuUI();
        contextMenu.SetupContextMenu();

        shaftSceneUI.UI_Item_Main();
    }

    private void Model()
    {
        moveSceneOnClick.SetupButtons();
        moveContextMenu.SetupMoveContextMenu();
        buyItemsForge.SetupBuyItems_forge();
        buyItemsStockExchange.SetupBuyItems_StockExchange();

        countersAnimation.SetupCountersAnimation();

        volumeController.SetupVolumeController();
    }

    private void Service()
    {
        webSocketConnect.ConnectToWebSocketServer();
    }

    private void OffBootstrapScene()
    {
        GameObject sceneManager = GameObject.Find("SceneManager");

        Debug.Log($"sceneManager : {sceneManager.name}");

        Transform bootstrapScene = sceneManager.GetComponent<Transform>().GetChild(0).GetComponent<Transform>();
        Canvas bootstrapSceneCanvas = bootstrapScene.GetComponent<Canvas>();

        bootstrapScene.gameObject.SetActive(false);
    }
}