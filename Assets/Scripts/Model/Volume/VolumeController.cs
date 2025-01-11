using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class VolumeController : MonoBehaviour
{
    [Inject] PlayerDataService playerDataService;

    [Header("Music")]
    [SerializeField] private TextMeshProUGUI musicVolume;

    [SerializeField] private Button musicMinusButton;
    [SerializeField]private Button musicPlusButton;

    [Header("Sound")]
    [SerializeField] private TextMeshProUGUI soundVolume;

    [SerializeField] private Button soundMinusButton;
    [SerializeField] private Button soundPlusButton;

    private AudioSource audioMusicSource;

    private PlayerDataOnSession playerDataOnSession;
    private ClicksController clicksController;

    [Inject]
    public void Construct(PlayerDataOnSession PlayerDataOnSession, ClicksController ClicksController)
    {
        playerDataOnSession = PlayerDataOnSession;
        clicksController = ClicksController;
    }

    public void SetupVolumeController()
    {
        GetAudioSource();

        musicPlusButton.onClick.AddListener(() =>
        {
            clicksController.ButtonClickAudio();
            ClickOnMusicButton("Plus music");
        });
        musicMinusButton.onClick.AddListener(() =>
        {
            clicksController.ButtonClickAudio();
            ClickOnMusicButton("Minus music");
        });

        soundPlusButton.onClick.AddListener(() =>
        {
            clicksController.ButtonClickAudio();
            ClickOnSoundButton("Plus sound");
        });
        soundMinusButton.onClick.AddListener(() =>
        {
            clicksController.ButtonClickAudio();
            ClickOnSoundButton("Minus sound");
        });
    }

    private void GetAudioSource()
    {
        GameObject audioSourceObj = GameObject.Find("SceneManager");
        audioMusicSource = audioSourceObj.GetComponent<AudioSource>();

        musicVolume.text = playerDataOnSession.music;
        soundVolume.text = playerDataOnSession.sound;
        audioMusicSource.volume = Convert.ToInt32(playerDataOnSession.music) / 100f;
    }

    private void ClickOnMusicButton(string operationSign)
    {
        int currentMusic;

        switch (operationSign)
        {
            case "Plus music":
                currentMusic = Convert.ToInt32(musicVolume.text) + 1;

                ClickOnButton(currentMusic, musicVolume, audioMusicSource);

                playerDataOnSession.UpdatePlayerMusic(musicVolume.text);
                break;
            case "Minus music":
                currentMusic = Convert.ToInt32(musicVolume.text) - 1;

                ClickOnButton(currentMusic, musicVolume, audioMusicSource);

                playerDataOnSession.UpdatePlayerMusic(musicVolume.text);
                break;
        }
    }
    private void ClickOnSoundButton(string operationSign)
    {
        int currentSound;

        switch (operationSign)
        {
            case "Plus sound":
                currentSound = Convert.ToInt32(soundVolume.text) + 1;

                ClickOnButton(currentSound, soundVolume, audioMusicSource);

                playerDataOnSession.UpdatePlayerMusic(soundVolume.text);
                break;
            case "Minus sound":
                currentSound = Convert.ToInt32(soundVolume.text) - 1;

                ClickOnButton(currentSound, soundVolume, audioMusicSource);

                playerDataOnSession.UpdatePlayerMusic(soundVolume.text);
                break;
        }
    }

    private void ClickOnButton(int currentSound, TextMeshProUGUI valume, AudioSource audioSource)
    {
        valume.text = Convert.ToString(currentSound);
        audioSource.volume = currentSound / 100f;
    }
}