using UnityEngine;

public class Setup_RegLogScene : MonoBehaviour
{
    [SerializeField] private DontDestroyOnLoadObject dontDestroyOnLoadObject;
    [SerializeField] private BaseCanvasUI baseCanvasUI;
    [SerializeField] private SignIn_LogIn_UI signInLogInUI;
    [SerializeField] private SignInLogInUser signInLogInUser;

    private void Awake()
    {
        View();
        Model();
    }

    private void View()
    {
        baseCanvasUI.BaseCanvas_UI();
        signInLogInUI.LogRegUI();
    }

    private void Model()
    {
        dontDestroyOnLoadObject.SetupDontDestroyOnLoad();
        signInLogInUser.SignIn_LogIn();
    }
}