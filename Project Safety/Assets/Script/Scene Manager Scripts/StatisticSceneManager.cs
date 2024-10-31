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
        FirebaseManager.Instance.FetchUserChoice("Act1Scene1_PlugChoice", choiceValue =>
        {
            int userChoice = choiceValue ?? PlayerPrefs.GetInt("Act1Scene1_PlugChoice", 0);
            FirebaseManager.Instance.FetchChoiceStatistics("Act1Scene1_PlugChoice", (percentage1, percentage2) =>
            {
                UpdatePlugChoiceUI(userChoice, percentage1, percentage2);
            });
        });

        FirebaseManager.Instance.FetchUserChoice("Act1Scene4_DummyChoice", choiceValue =>
        {
            int userChoice = choiceValue ?? PlayerPrefs.GetInt("Act1Scene4_DummyChoice", 0);

            FirebaseManager.Instance.FetchChoiceStatistics("Act1Scene4_DummyChoice", (percentage1, percentage2) =>
            {
                UpdateDummyChoiceUI(userChoice, percentage1, percentage2);
            });
        });

        FirebaseManager.Instance.FetchUserChoice("Act2Scene1_CallChoice", choiceValue =>
        {
            int userChoice = choiceValue ?? PlayerPrefs.GetInt("Act2Scene1_CallChoice", 0); 
            FirebaseManager.Instance.FetchChoiceStatisticsThreeOptions("Act2Scene1_CallChoice", (percentage1, percentage2, percentage3) =>
            {
                UpdateCallChoiceUI(userChoice, percentage1, percentage2, percentage3);
            });
        });

        FirebaseManager.Instance.FetchUserChoice("Act2Scene2_GatherBelongingsChoice", choiceValue =>
        {
            int userChoice = choiceValue ?? PlayerPrefs.GetInt("Act2Scene2_GatherBelongingsChoice", 0);

            FirebaseManager.Instance.FetchChoiceStatistics("Act2Scene2_GatherBelongingsChoice", (percentage1, percentage2) =>
            {
                UpdateGatherBelongingsChoiceUI(userChoice, percentage1, percentage2);
            });
        });

        FirebaseManager.Instance.FetchUserChoice("Act2Scene2_ElevatorChoice", choiceValue =>
        {
            int userChoice = choiceValue ?? PlayerPrefs.GetInt("Act2Scene2_ElevatorChoice", 0);

            FirebaseManager.Instance.FetchChoiceStatistics("Act2Scene2_ElevatorChoice", (percentage1, percentage2) =>
            {
                UpdateElevatorChoiceUI(userChoice, percentage1, percentage2);
            });
        });
    }

    private void UpdatePlugChoiceUI(int userChoice, int percentage1, int percentage2)
    {
        Debug.Log($"Updating Plug Choice UI. User choice: {userChoice}, Percentages: {percentage1}, {percentage2}");

        switch (userChoice)
        {
            case 1:
                plugChoiceSlider.value = percentage1 / 100f;
                plugChoiceText.text = $"You and {percentage1}% chose to unplug the devices in your room.";
                break;
            case 2:
                plugChoiceSlider.value = percentage2 / 100f;
                plugChoiceText.text = $"You and {percentage2}% got scolded by your mom.";
                break;
            default:
                plugChoiceText.text = "No valid choice recorded.";
                break;
        }
    }

    private void UpdateDummyChoiceUI(int userChoice, int percentage1, int percentage2)
    {
        Debug.Log($"Updating Dummy Choice UI. User choice: {userChoice}, Percentages: {percentage1}, {percentage2}");

        switch (userChoice)
        {
            case 1:
                dummyChoiceSlider.value = percentage1 / 100f;
                dummyChoiceText.text = $"You and {percentage1}% chose to save the dummy.";
                break;
            case 2:
                dummyChoiceSlider.value = percentage2 / 100f;
                dummyChoiceText.text = $"You and {percentage2}% did not save the dummy.";
                break;
            default:
                dummyChoiceText.text = "No valid choice recorded.";
                break;
        }
    }

    private void UpdateCallChoiceUI(int userChoice, int percentage1, int percentage2, int percentage3)
    {
        Debug.Log($"Updating Call Choice UI. User choice: {userChoice}, Percentages: {percentage1}, {percentage2}, {percentage3}");

        // Update UI based on the player's choice
        switch (userChoice)
        {
            case 1:
                callChoiceSlider.value = percentage1 / 100f;
                callChoiceText.text = $"You and {percentage1}% of players called the local fire station.";
                break;
            case 2:
                callChoiceSlider.value = percentage2 / 100f;
                callChoiceText.text = $"You and {percentage2}% of players called the national emergency hotline.";
                break;
            case 3:
                callChoiceSlider.value = percentage3 / 100f;
                callChoiceText.text = $"You and {percentage3}% of players called mom.";
                break;
            default:
                callChoiceText.text = "No valid choice recorded.";
                break;
        }
    }


    private void UpdateGatherBelongingsChoiceUI(int userChoice, int percentage1, int percentage2)
    {
        Debug.Log($"Updating Gather Belongings Choice UI. User choice: {userChoice}, Percentages: {percentage1}, {percentage2}");

        switch (userChoice)
        {
            case 1:
                plugChoiceSlider.value = percentage1 / 100f;
                plugChoiceText.text = $"You and {percentage1}% of players are materialistic.";
                break;
            case 2:
                plugChoiceSlider.value = percentage2 / 100f;
                plugChoiceText.text = $"You and {percentage2}% of players are realistic.";
                break;
            default:
                plugChoiceText.text = "No valid choice recorded.";
                break;
        }
    }

    private void UpdateElevatorChoiceUI(int userChoice, int percentage1, int percentage2)
    {
        Debug.Log($"Updating Elevator Choice UI. User choice: {userChoice}, Percentages: {percentage1}, {percentage2}");

        switch (userChoice)
        {
            case 1:
                plugChoiceSlider.value = percentage1 / 100f;
                plugChoiceText.text = $"You and {percentage1}% of players first fell to their death.";
                break;
            case 2:
                plugChoiceSlider.value = percentage2 / 100f;
                plugChoiceText.text = $"You and {percentage2}% of players went down the stairs.";
                break;
            default:
                plugChoiceText.text = "No valid choice recorded.";
                break;
        }
    }
}
