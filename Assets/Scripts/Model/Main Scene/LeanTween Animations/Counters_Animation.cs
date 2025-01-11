using System.Collections.Generic;
using UnityEngine;
using Zenject;
using TMPro;

public class Counters_Animation : MonoBehaviour
{
    [Inject] private PostRequest postRequest;

    [SerializeField] private float speedAnimation = 2f;

    [SerializeField] private TextMeshProUGUI diamondCounter;
    [SerializeField] private TextMeshProUGUI goldCounter;
    [SerializeField] private TextMeshProUGUI eriCounter;

    public Dictionary<string, string> gameData { get; private set; } = new Dictionary<string, string>();

    private string currentDiamond = "0";
    private string currentGold = "0";

    public void SetupCountersAnimation()
    {
        gameData = postRequest.GetGameDataData();

        string diamond = gameData["diamond"];
        string gold = gameData["gold"];

        UpdateDiamondCounter(diamond);
        UpdateGoldCounter(gold);
    }

    public void UpdateGoldVariable(string newGold)
    {
        currentGold = newGold;
        goldCounter.text = newGold;
    }

    public void AnimateCounter(TextMeshProUGUI counter, string startValue, string endValue, System.Action<string> updateAction)
    {
        float startFloat = ParseStringToFloat(startValue);
        float endFloat = ParseStringToFloat(endValue);

        LeanTween.value(startFloat, endFloat, speedAnimation)
            .setOnUpdate((float value) =>
            {
                string formattedValue = FormatFloatToString(value);
                updateAction?.Invoke(formattedValue);
            })
            .setOnComplete(() =>
            {
                updateAction?.Invoke(endValue);
            });
    }

    public void UpdateDiamondCounter(string newDiamond)
    {
        AnimateCounter(diamondCounter, currentDiamond, newDiamond, value =>
        {
            diamondCounter.text = value;
            currentDiamond = value;
        });
    }

    private void UpdateGoldCounter(string newGold)
    {
        AnimateCounter(goldCounter, currentGold, newGold, value =>
        {
            goldCounter.text = value;
            currentGold = value;
        });
    }

    private float ParseStringToFloat(string value)
    {
        if (value.EndsWith("k"))
        {
            float.TryParse(value.TrimEnd('k'), out float result);
            return result * 1000f;
        }
        if (value.EndsWith("m"))
        {
            float.TryParse(value.TrimEnd('m'), out float result);
            return result * 1000000f;
        }

        float.TryParse(value, out float numericValue);
        return numericValue;
    }

    private string FormatFloatToString(float value)
    {
        if (value >= 1000000f)
        {
            return (value / 1000000f).ToString("0.##") + "m";
        }
        if (value >= 1000f)
        {
            return (value / 1000f).ToString("0.##") + "k";
        }

        return Mathf.RoundToInt(value).ToString();
    }
}