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
    }

    void Update()
    {
        if(!audioRepeat)
        {
            CheckPlayerAudioPlaying();
        }

       VehicleLerp();

        // if (HUDManager.instance.instructionHUD.activeSelf)
        // {
        //     DeviceChecker();
        // }
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
