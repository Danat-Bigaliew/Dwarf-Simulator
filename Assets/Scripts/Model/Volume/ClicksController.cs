using UnityEngine;

public class ClicksController : MonoBehaviour
{
    private AudioSource clickOnButtonAudio;
    private AudioSource clickOnMainSceneButtonAudio;

    public void Awake()
    {
        AudioSource[] audioSorces = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);

        foreach (AudioSource childAudioSorces in audioSorces)
        {
            switch (childAudioSorces.clip.name)
            {
                case "ButtonClick":
                    clickOnButtonAudio = childAudioSorces;
                    break;
                case "MainSceneClick":
                    clickOnMainSceneButtonAudio = childAudioSorces;
                    break;
            }
        }
    }

    public void ButtonClickAudio()
    {
        clickOnButtonAudio.Play();
    }
    public void ButtonClickAudioInMainScene()
    {
        clickOnMainSceneButtonAudio.Play();
    }
}