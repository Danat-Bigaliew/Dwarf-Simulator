using UnityEngine;

public class PlayerDataOnSession : MonoBehaviour, PlayerDataService
{
    public string playerKey { get; private set; }

    public string music { get; private set; } = "20";
    public string sound { get; private set; } = "20";

    public void SetPlayerKey(string key)
    {
        playerKey = key;
    }

    public void UpdatePlayerMusic(string newMusic)
    {
        music = newMusic;
    }

    public void UpdatePlayerSound(string newSound)
    {
        sound = newSound;
    }
}