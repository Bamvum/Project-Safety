using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class StatisticSceneManager : MonoBehaviour
{
    private int language;
    PlayerControls playerControls;

    [Header("HUD")]
    [SerializeField] RectTransform statisticRectTransform;
    [SerializeField] CanvasGroup statisticButtonCG;
    [SerializeField] RectTransform statisticAdditionalInformationRectTransform;
    [SerializeField] TMP_Text statisticAdditionalInformationText;
    [SerializeField] TMP_Text statisticNavGuide;

    
    [Header("SELECTED BUTTON")]
    [SerializeField] GameObject lastSelectedButton; // FOR GAMEPAD

    [Space(5)]
    [SerializeField] GameObject statisticSelectedButton;

    [Header("FLAG")]
    [SerializeField] bool isGamepad;
    [SerializeField] bool canInput;

    public Slider plugChoiceSlider;
    public TextMeshProUGUI plugChoiceText;
    public TextMeshProUGUI plugChoicedescText;

    public Slider dummyChoiceSlider;
    public TextMeshProUGUI dummyChoiceText;
    public TextMeshProUGUI dummyChoicedescText;

    public Slider callChoiceSlider;
    public TextMeshProUGUI callChoiceText;
    public TextMeshProUGUI callChoicedescText;

    public Slider gatherBelongingsSlider;
    public TextMeshProUGUI gatherBelongingsText;
    public TextMeshProUGUI gatherBelongingsdescText;

    public Slider elevatorChoiceSlider;
    public TextMeshProUGUI elevatorChoiceText;
    public TextMeshProUGUI elevatorChoicedescText;


    void Awake()
    {
        playerControls = new PlayerControls();
    }

    void OnEnable()
    {
        playerControls.Statistics.Back.performed += ToBack;
        playerControls.Statistics.ToMainMenu.performed += ToMainMenu;
        playerControls.Statistics.ToPostAssessment.performed += ToPostAssessment;
        
        playerControls.Statistics.Enable();
    }

    void OnDisable()
    {
        playerControls.Statistics.Disable();
    }

    private void ToBack(InputAction.CallbackContext context)
    {
        if(statisticAdditionalInformationRectTransform.gameObject.activeSelf)
        {
            HideAdditionalInformation();
        }
    }

    private void ToMainMenu(InputAction.CallbackContext context)
    {
        if (!statisticAdditionalInformationRectTransform.gameObject.activeSelf)
        {
            if(canInput)
            {
                Debug.Log("To Main Menu");

                canInput = false;

                statisticButtonCG.interactable = false;

                Cursor.lockState = CursorLockMode.Locked;

                LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);
                LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
                .SetEase(Ease.Linear)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    LoadingSceneManager.instance.loadingScreen.SetActive(true);
                    LoadingSceneManager.instance.enabled = true;
                    LoadingSceneManager.instance.sceneName = "Main Menu";
                });
            }

        }
    }

    private void ToPostAssessment(InputAction.CallbackContext context)
    {
        if (!statisticAdditionalInformationRectTransform.gameObject.activeSelf)
        {
            if (canInput)
            {
                Debug.Log("To Main Menu");
                
                canInput = false;
        
                statisticButtonCG.interactable = false;

                Cursor.lockState = CursorLockMode.Locked;

                LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);
                LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
                    .SetEase(Ease.Linear)
                    .SetUpdate(true)
                    .OnComplete(() =>
                    {
                        LoadingSceneManager.instance.loadingScreen.SetActive(true);
                        LoadingSceneManager.instance.enabled = true;
                        LoadingSceneManager.instance.sceneName = "Post Assessment";
                    });
            }
        }

    }

    void Update()
    {
        DeviceInputCheckerUI();
        DeviceInputCheckerNavGuide();
        

        // GAMEPAD VIBRATION ON NAVIGATION 
        if (Gamepad.current != null && EventSystem.current.currentSelectedGameObject != null)
        {
            if (Gamepad.current.leftStick.ReadValue() != Vector2.zero || Gamepad.current.dpad.ReadValue() != Vector2.zero)
            {
                GameObject currentSelectedButton = EventSystem.current.currentSelectedGameObject;

                Debug.Log("Inputed Leftstick and Dpad");
                // Check if the selected UI element has changed (button navigation)
                if (currentSelectedButton != lastSelectedButton)
                {
                    // Trigger vibration when navigating to a new button
                    VibrateGamepad();
                    lastSelectedButton = currentSelectedButton; // Update the last selected button
                }
            }
        }
    }

    private void VibrateGamepad()
    {
        // Set a short vibration
        Gamepad.current.SetMotorSpeeds(0.3f, 0.3f); // Adjust the intensity here
        Invoke("StopVibration", 0.1f); // Stops vibration after 0.1 seconds
        StartCoroutine(StopVibration(.1f));
    }


    IEnumerator StopVibration(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Gamepad.current.SetMotorSpeeds(0, 0);
    }

    #region - DEVICE CHECKER [HUD/UI] -

    public void DeviceInputCheckerUI()
    {
        if (DeviceManager.instance.keyboardDevice)
        {
            if (statisticRectTransform.gameObject.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                EventSystem.current.SetSelectedGameObject(null);
                isGamepad = false;
            }
        }
        else if (DeviceManager.instance.gamepadDevice)
        {
            Cursor.lockState = CursorLockMode.Locked;

            if (!isGamepad)
            {
                if (statisticRectTransform.gameObject.activeSelf)
                {
                    EventSystem.current.SetSelectedGameObject(statisticSelectedButton);
                    isGamepad = true;
                }
            }
        }
    }

    #endregion

    #region - NAVIGATION GUIDE -

    void DeviceInputCheckerNavGuide()
    {
        if (DeviceManager.instance.keyboardDevice)
        {
            statisticNavGuide.text = "<sprite name=\"1\"> Proceed to Post-Assessment <sprite name=\"2\"> Main Menu <sprite name=\"Left Mouse Button\"> Show Additional Information <sprite name=\"Escape\"> Back   ";
        }
        else if (DeviceManager.instance.gamepadDevice)
        {
            statisticNavGuide.text = "<sprite name=\"Square\"> Proceed to Post-Assessment <sprite name=\"Triangle\"> Main Menu <sprite name=\"Cross\"> Show Additional Information <sprite name=\"Circle\"> Back   ";
        }
    }

    #endregion

    void Start()
    {
        FirebaseManager firebaseManager = FindObjectOfType<FirebaseManager>();

        if (firebaseManager != null)
        {
            // Fetch settings, including language, from Firebase
            firebaseManager.FetchSettingsFromFirebase(OnSettingsFetched);
        }
        else
        {
            Debug.LogError("FirebaseManager not found in the scene. Ensure it is accessible.");
        }
        language = PlayerPrefs.GetInt("Language", 0);
        Debug.Log($"Language setting on Statistics Scene load: {language}");
        ApplyLanguageToUI(language);

        FirebaseManager.Instance.FetchUserChoice("Act1Scene1_PlugChoice", choiceValue =>
        {
            int userChoice = choiceValue ?? PlayerPrefs.GetInt("Act1Scene1_PlugChoice", 0);
            FirebaseManager.Instance.FetchChoiceStatistics("Act1Scene1_PlugChoice", (percentage1, percentage2) =>
            {
                UpdatePlugChoiceUI(userChoice, percentage1, percentage2, language);
            });
        });

        FirebaseManager.Instance.FetchUserChoice("Act1Scene4_DummyChoice", choiceValue =>
        {
            int userChoice = choiceValue ?? PlayerPrefs.GetInt("Act1Scene4_DummyChoice", 0);

            FirebaseManager.Instance.FetchChoiceStatistics("Act1Scene4_DummyChoice", (percentage1, percentage2) =>
            {
                UpdateDummyChoiceUI(userChoice, percentage1, percentage2, language);
            });
        });

        FirebaseManager.Instance.FetchUserChoice("Act2Scene1_CallChoice", choiceValue =>
        {
            int userChoice = choiceValue ?? PlayerPrefs.GetInt("Act2Scene1_CallChoice", 0); 
            FirebaseManager.Instance.FetchChoiceStatisticsThreeOptions("Act2Scene1_CallChoice", (percentage1, percentage2, percentage3) =>
            {
                UpdateCallChoiceUI(userChoice, percentage1, percentage2, percentage3, language);
            });
        });

        FirebaseManager.Instance.FetchUserChoice("Act2Scene2_GatherBelongingsChoice", choiceValue =>
        {
            int userChoice = choiceValue ?? PlayerPrefs.GetInt("Act2Scene2_GatherBelongingsChoice", 0);

            FirebaseManager.Instance.FetchChoiceStatistics("Act2Scene2_GatherBelongingsChoice", (percentage1, percentage2) =>
            {
                UpdateGatherBelongingsChoiceUI(userChoice, percentage1, percentage2, language);
            });
        });

        FirebaseManager.Instance.FetchUserChoice("Act2Scene2_ElevatorChoice", choiceValue =>
        {
            int userChoice = choiceValue ?? PlayerPrefs.GetInt("Act2Scene2_ElevatorChoice", 0);

            FirebaseManager.Instance.FetchChoiceStatistics("Act2Scene2_ElevatorChoice", (percentage1, percentage2) =>
            {
                UpdateElevatorChoiceUI(userChoice, percentage1, percentage2, language);
            });
        });

        PlayerPrefs.SetInt("Post-Assessment", 1);
        PlayerPrefs.SetInt("Statistics", 1);
        FirebaseManager.Instance.SaveChapterUnlockToFirebase("Post-Assessment", true);
        FirebaseManager.Instance.SaveChapterUnlockToFirebase("Statistics", true);

        StartCoroutine(FadeOutEffect());
    }

    IEnumerator FadeOutEffect()
    {
        yield return new WaitForSeconds(1);
        LoadingSceneManager.instance.fadeImage.DOFade(0, LoadingSceneManager.instance.fadeDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                LoadingSceneManager.instance.fadeImage.gameObject.SetActive(false);
                statisticButtonCG.interactable = true;
            });

    }

    #region - STATISTIC Data Set - 

    private void OnSettingsFetched(
    float masterVolume, float musicVolume, float voiceVolume, float sfxVolume, bool isFullScreen,
    int qualityIndex, int resolutionIndex, float xMouseSens, float yMouseSens,
    float xGamepadSens, float yGamepadSens, float dialogueSpeed, int languageIndex)
    {
        // Save the language index to PlayerPrefs (optional, if used elsewhere)
        PlayerPrefs.SetInt("Language", languageIndex);
        PlayerPrefs.Save();

        Debug.Log($"Fetched language index from Firebase: {languageIndex}");
    }

    private void ApplyLanguageToUI(int language)
    {
        if (language == 1)
        {
            Debug.Log("Applying Tagalog language settings to UI.");
        }
        else
        {
            Debug.Log("Applying English language settings to UI.");
        }
    }

    private void UpdatePlugChoiceUI(int userChoice, int percentage1, int percentage2, int language)
{
    string unplugText, scoldedText, descriptionText;

    if (language == 0)
    {
        unplugText = $"You and {percentage1}% chose to unplug the devices in your room.";
        scoldedText = $"You and {percentage2}% got scolded by mom.";
        descriptionText = "Did you unplug the devices in your room on your own?";
    }
    else
    {
        unplugText = $"Ikaw at {percentage1}% ay nagtanggal ng mga aparato sa iyong silid.";
        scoldedText = $"Ikaw at {percentage2}% ay pinagalitan ng nanay.";
        descriptionText = "Ikaw ba'y nagtanggal ng mga aparato sa sarili mong kusa?";
    }

    switch (userChoice)
    {
        case 1:
            plugChoiceSlider.value = percentage1 / 100f;
            plugChoiceText.text = unplugText;
            plugChoicedescText.text = descriptionText;
            break;
        case 2:
            plugChoiceSlider.value = percentage2 / 100f;
            plugChoiceText.text = scoldedText;
            plugChoicedescText.text = descriptionText;
            break;
        default:
            plugChoiceText.text = "-";
            plugChoicedescText.text = "-";
            break;
    }
}

    private void UpdateDummyChoiceUI(int userChoice, int percentage1, int percentage2, int language)
    {
        Debug.Log($"Updating Dummy Choice UI. User choice: {userChoice}, Percentages: {percentage1}, {percentage2}");

        string heroText, selfText, descriptionText;

        if (language == 0) 
        {
            heroText = $"You and {percentage1}% tried to be a hero.";
            selfText = $"You and {percentage2}% looked out for themselves.";
            descriptionText = "Did you try to save the training dummy?";
        }
        else 
        {
            heroText = $"Ikaw at {percentage1}% ay nagpakabayani.";
            selfText = $"Ikaw at {percentage2}% ay nag-isip ng sariling kaligtasan.";
            descriptionText = "Sinubukan mo bang iligtas ang training dummy?";
        }

        switch (userChoice)
        {
            case 1:
                dummyChoiceSlider.value = percentage1 / 100f;
                dummyChoiceText.text = heroText;
                dummyChoicedescText.text = descriptionText;
                break;
            case 2:
                dummyChoiceSlider.value = percentage2 / 100f;
                dummyChoiceText.text = selfText;
                dummyChoicedescText.text = descriptionText;
                break;
            default:
                dummyChoiceText.text = "-";
                dummyChoicedescText.text = "-";
                break;
        }
    }

    private void UpdateCallChoiceUI(int userChoice, int percentage1, int percentage2, int percentage3, int language)
    {
        Debug.Log($"Updating Call Choice UI. User choice: {userChoice}, Percentages: {percentage1}, {percentage2}, {percentage3}");

        string fireStationText, hotlineText, momText, descriptionText;

        if (language == 0)
        {
            fireStationText = $"You and {percentage1}% of players called the local fire station.";
            hotlineText = $"You and {percentage2}% of players called the national emergency hotline.";
            momText = $"You and {percentage3}% of players called mom.";
            descriptionText = "Who did you call first for help?";
        }
        else
        {
            fireStationText = $"Ikaw at {percentage1}% ay tumawag sa lokal na bumbero.";
            hotlineText = $"Ikaw at {percentage2}% ay tumawag sa pambansang emergency hotline.";
            momText = $"Ikaw at {percentage3}% ay tumawag kay nanay.";
            descriptionText = "Sino ang una mong tinawagan para humingi ng tulong?";
        }

        switch (userChoice)
        {
            case 1:
                callChoiceSlider.value = percentage1 / 100f;
                callChoiceText.text = fireStationText;
                callChoicedescText.text = descriptionText;
                break;
            case 2:
                callChoiceSlider.value = percentage2 / 100f;
                callChoiceText.text = hotlineText;
                callChoicedescText.text = descriptionText;
                break;
            case 3:
                callChoiceSlider.value = percentage3 / 100f;
                callChoiceText.text = momText;
                callChoicedescText.text = descriptionText;
                break;
            default:
                callChoiceText.text = "-";
                callChoicedescText.text = "-";
                break;
        }
    }


    private void UpdateGatherBelongingsChoiceUI(int userChoice, int percentage1, int percentage2, int language)
    {
        Debug.Log($"Updating Gather Belongings Choice UI. User choice: {userChoice}, Percentages: {percentage1}, {percentage2}");

        string gatheredText, leftImmediatelyText, descriptionText;

        if (language == 0)
        {
            gatheredText = $"You and {percentage1}% of players gathered their belongings first.";
            leftImmediatelyText = $"You and {percentage2}% of players left immediately.";
            descriptionText = "Did you leave immediately during an emergency?";
        }
        else
        {
            gatheredText = $"Ikaw at {percentage1}% ay nagtipon ng kanilang mga gamit muna.";
            leftImmediatelyText = $"Ikaw at {percentage2}% ay agad na umalis.";
            descriptionText = "Ikaw ba'y agad umalis sa oras ng emergency?";
        }

        switch (userChoice)
        {
            case 1:
                gatherBelongingsSlider.value = percentage1 / 100f;
                gatherBelongingsText.text = gatheredText;
                gatherBelongingsdescText.text = descriptionText;
                break;
            case 2:
                gatherBelongingsSlider.value = percentage2 / 100f;
                gatherBelongingsText.text = leftImmediatelyText;
                gatherBelongingsdescText.text = descriptionText;
                break;
            default:
                gatherBelongingsText.text = "-";
                gatherBelongingsdescText.text = "-";
                break;
        }
    }

    private void UpdateElevatorChoiceUI(int userChoice, int percentage1, int percentage2, int language)
    {
        Debug.Log($"Updating Elevator Choice UI. User choice: {userChoice}, Percentages: {percentage1}, {percentage2}");

        string elevatorText, stairsText, descriptionText;

        if (language == 0)
        {
            elevatorText = $"You and {percentage1}% of players rode the elevator.";
            stairsText = $"You and {percentage2}% of players went down the stairs.";
            descriptionText = "Which path did you take first to the lower floors?";
        }
        else
        {
            elevatorText = $"Ikaw at {percentage1}% ay sumakay ng elevator.";
            stairsText = $"Ikaw at {percentage2}% ay bumaba sa hagdan.";
            descriptionText = "Aling daan ang una mong tinahak papunta sa ibabang palapag?";
        }

        switch (userChoice)
        {
            case 1:
                elevatorChoiceSlider.value = percentage1 / 100f;
                elevatorChoiceText.text = elevatorText;
                elevatorChoicedescText.text = descriptionText;
                break;
            case 2:
                elevatorChoiceSlider.value = percentage2 / 100f;
                elevatorChoiceText.text = stairsText;
                elevatorChoicedescText.text = descriptionText;
                break;
            default:
                elevatorChoiceText.text = "-";
                elevatorChoicedescText.text = "-";
                break;
        }
    }

    #endregion 

    #region - STATISTIC ADDITIONAL INFORMATION - 

    public void ShowAdditionalInformation(StatisticSO statisticSO)
    {
        if (language == 0)
        {
            statisticAdditionalInformationText.text = statisticSO.englishStatisticAdditionalInformation;
        }
        else
        {
            statisticAdditionalInformationText.text = statisticSO.tagalogStatisticAdditionalInformation;
        }

        // RECT TRANSFORM OF THINGS
        statisticButtonCG.interactable = false;
        isGamepad = true;

        statisticAdditionalInformationRectTransform.gameObject.SetActive(true);
        statisticAdditionalInformationRectTransform.DOScale(Vector3.one, .75f).OnComplete(() =>
        {
            statisticAdditionalInformationRectTransform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 10, 1)
                .SetEase(Ease.InFlash)
                .SetUpdate(true);
        });


    }

    void HideAdditionalInformation()
    {
        // RECT TRANSFORM HIDE
        statisticAdditionalInformationRectTransform.DOScale(Vector3.zero, .75f).OnComplete(() =>
           {
               statisticAdditionalInformationRectTransform.gameObject.SetActive(false);
               statisticButtonCG.interactable = true;
               // statisticAdditionalInformationText.text = null;

           });
    }

    #endregion 

}
