using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class StatisticSceneManager : MonoBehaviour
{
    public Slider plugChoiceSlider;
    public TextMeshProUGUI plugChoiceText;

    public Slider dummyChoiceSlider;
    public TextMeshProUGUI dummyChoiceText;

    public Slider callChoiceSlider;
    public TextMeshProUGUI callChoiceText;

    public Slider gatherBelongingsSlider;
    public TextMeshProUGUI gatherBelongingsText;

    public Slider elevatorChoiceSlider;
    public TextMeshProUGUI elevatorChoiceText;

    void Start()
    {
        FirebaseManager.Instance.FetchChoiceStatistics("Act1Scene1_PlugChoice", UpdatePlugChoiceUI);
    }

    private void UpdatePlugChoiceUI(int percentage)
    {
        // Set slider to a value between 0 and 1

        // Display text based on the player's saved choice
        if (PlayerPrefs.GetInt("Act1Scene1_PlugChoice") == 1)
        {
            plugChoiceText.text = $"You and {percentage}% chose to unplug the devices in your room.";
            plugChoiceSlider.value = percentage / 100f;
        }
        else
        {
            int oppositePercentage = 100 - percentage;
            plugChoiceText.text = $"You and {oppositePercentage}% got scolded by your mom.";
            plugChoiceSlider.value = oppositePercentage / 100f;
        }
    }
}
