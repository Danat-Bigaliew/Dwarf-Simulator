using System.Collections.Generic;

public interface PostRequest
{
    void SignInLogIn(string login, string password, string nickname, string adress = "/");

    Dictionary<string, string> GetForgeData();
    Dictionary<string, string> GetStockExchangeData();
    public Dictionary<string, string> GetGameDataData();
}