using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Act1Scene2SceneManager : MonoBehaviour
{
    public static Act1Scene2SceneManager instance {get; private set;}
    
    [Header("Dialogue Triggers")]
    [SerializeField] DialogueTrigger startDialogueTrigger;

    [Header("Dialogue Triggers")]
    [SerializeField] GameObject instructionHUD;
    [SerializeField] RectTransform instructionBGRectTransform;
    [SerializeField] GameObject instructionContent;
    [SerializeField] CanvasGroup instructionContentCG;
    

    [Space(10)]
    [SerializeField] Image  instructionImg;
    [Space(10)]
    [SerializeField] Sprite[] instructionSprite;

    [Space(10)]
    [SerializeField] AudioSource fireTruckSirenSFX;
    [SerializeField] GameObject bus;
    [SerializeField] GameObject firetruck;
    
    [Space(10)]
    [SerializeField] AudioSource doorPlayerAudio;
    [SerializeField] AudioClip doorSFX;

    [Header("Flag")]
    bool audioRepeat;
    [SerializeField] bool lerpFireTruck;
    [SerializeField] bool lerpBus;

    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Start()
    {
        // FADE IMAGE ALPHA SET 1
        LoadingSceneManager.instance.fadeImage.color = new Color(LoadingSceneManager.instance.fadeImage.color.r,
                                                                LoadingSceneManager.instance.fadeImage.color.g,
                                                                LoadingSceneManager.instance.fadeImage.color.b,
                                                                1);

        instructionBGRectTransform.sizeDelta = new Vector2(0, instructionBGRectTransform.sizeDelta.y);
        instructionContentCG.alpha = 0;
        // TODO - SFX OF DOOR

    } 

    void Update()
    {
        if(!audioRepeat)
        {
            CheckPlayerAudioPlaying();
        }

       VehicleLerp();

        if (HUDManager.instance.instructionHUD.activeSelf)
        {
            DeviceChecker();
        }
    }

    void CheckPlayerAudioPlaying()
    {
        if(!doorPlayerAudio.isPlaying)
        {
            // FADEOUT EFFECTS
            LoadingSceneManager.instance.fadeImage
                .DOFade(0, LoadingSceneManager.instance.fadeDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
            {
                LoadingSceneManager.instance.fadeImage.gameObject.SetActive(false);

                audioRepeat = true;
                startDialogueTrigger.StartDialogue();
            });
        }
    }

    void DeviceChecker()
    {
        if(DeviceManager.instance.keyboardDevice)
        {
            instructionImg.sprite = instructionSprite[0];
        }
        else if(DeviceManager.instance.gamepadDevice)
        {
            instructionImg.sprite = instructionSprite[1];
        }
    }

    public void DisplayInstruction()
    {
        // Time. timeScale = 0;
        instructionHUD.SetActive(true);

        instructionBGRectTransform.DOSizeDelta(new Vector2(1920, instructionBGRectTransform.sizeDelta.y), .5f)
            .SetEase(Ease.InQuad)
            .SetUpdate(true)
            .OnComplete(() =>
        {
            instructionContent.SetActive(true);
            instructionContentCG.DOFade(1, .75f).SetUpdate(true);
        });
    }

    public void HideInstruction()
    {
        Time.timeScale = 1;
        instructionContentCG.DOFade(1, .75f).OnComplete(() =>
        {
            instructionContent.SetActive(false);
            instructionBGRectTransform.DOSizeDelta(new Vector2(0, instructionBGRectTransform.sizeDelta.y), .5f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
            {
                instructionHUD.SetActive(false);

                //ENABLE SCRIPT
                PlayerScript.instance.playerMovement.enabled = true;
                // PlayerScript.instance.playerMovement.playerAnim.enabled = true;
                PlayerScript.instance.cinemachineInputProvider.enabled = true;
                PlayerScript.instance.interact.enabled = true;
                // PlayerScript.instance.examine.enabled = true;

                // Cursor.lockState = CursorLockMode.Locked;

                HUDManager.instance.playerHUD.SetActive(true);
                MissionManager.instance.DisplayMission();
            });
        });
    }

    void VehicleLerp()
    {
        if (lerpFireTruck)
        {
            if (!fireTruckSirenSFX.isPlaying)
            {
                fireTruckSirenSFX.Play();
            }

            firetruck.transform.position = Vector3.MoveTowards(firetruck.transform.position, new Vector3(107, firetruck.transform.position.y, 100), Time.deltaTime * 25f);

            if (firetruck.transform.position == new Vector3(107, firetruck.transform.position.y, 100))
            {
                lerpFireTruck = false;
                Destroy(firetruck);
            }
        }

        if (lerpBus)
        {
            bus.transform.position = Vector3.MoveTowards(bus.transform.position, new Vector3(107, bus.transform.position.y, 100), Time.deltaTime * 25f);
            if (bus.transform.position == new Vector3(107, bus.transform.position.y, 100))
            {
                lerpBus = false;
            }
        }
    }

    public void enableFireTruckLerp(bool enable)
    {
        lerpFireTruck = enable;
    }

    public void enableBusLerp(bool enable)
    {
        bus.SetActive(true);
        lerpBus = enable;
    }

}
