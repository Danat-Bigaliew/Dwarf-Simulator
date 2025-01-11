using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;

public class SignInLogInUser : MonoBehaviour
{
    [Inject] PostRequest postRequest;

    private Transform buttonsContainer;

    [SerializeField] private TextMeshProUGUI loginText;
    [SerializeField] private TextMeshProUGUI passwordText;
    [SerializeField] private TextMeshProUGUI nicknameText;

    private Button signInButton;
    private Button logInButton;

    private string SignInAdress = "/registrationUser";
    private string LogInAdress = "/authorizationUser";

    private ClicksController clicksController;

    [Inject]
    public void Construct(ClicksController ClicksController)
    {
        clicksController = ClicksController;
    }

    public void SignIn_LogIn()
    {
        buttonsContainer = GetComponent<Transform>();

        signInButton = buttonsContainer.GetChild(0).GetComponent<Button>();
        logInButton = buttonsContainer.GetChild(1).GetComponent<Button>();

        signInButton.onClick.AddListener(() =>
        {
            clicksController.ButtonClickAudio();
            postRequest.SignInLogIn(loginText.text, passwordText.text, nicknameText.text, SignInAdress);

        });
        logInButton.onClick.AddListener(() =>
        {
            clicksController.ButtonClickAudio();
            postRequest.SignInLogIn(loginText.text, passwordText.text, nicknameText.text, LogInAdress);
        });
    }
}