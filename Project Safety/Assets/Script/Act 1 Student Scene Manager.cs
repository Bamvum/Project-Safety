using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Act1StudentSceneManager : MonoBehaviour
{
    public static Act1StudentSceneManager instance {get; private set;}

    void Awake()
    {
        instance = this;
    }

    [SerializeField] MissionSO missionSO;

    [Header("Dialogue Triggers")]
    [SerializeField] DialogueTrigger startDialogueTrigger;

    [Space(15)]
    [SerializeField] AudioSource bedPlayerAudio;
    [SerializeField] AudioClip heavyBreathingSFX;

    [Header("Flag")]
    bool audioRepeat;
    int missionIndex;

    void Start()
    {
        // FADE IMAGE ALPHA SET 1
        LoadingSceneManager.instance.fadeImage.color = new Color(LoadingSceneManager.instance.fadeImage.color.r,
                                                                LoadingSceneManager.instance.fadeImage.color.g,
                                                                LoadingSceneManager.instance.fadeImage.color.b,
                                                                1);

        bedPlayerAudio.clip = heavyBreathingSFX;
        bedPlayerAudio.Play();
    
    }

    void Update()
    {
        // SCENE FLOW   -   PLAYER WAKES UP
        //              -   HEAVY BREATHING 
        //              -   
        //              -   

        if(!audioRepeat)
        {
            CheckPlayerAudioPlaying();
        }
    }

    void CheckPlayerAudioPlaying()
    {
        if(!bedPlayerAudio.isPlaying)
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

    #region - MISSION -

    public void DisplayMission()
    {
        HUDManager.instance.missionText.text = missionSO.missions[missionIndex];
        MissionManager.instance.missionSFX.Play();

        HUDManager.instance.missionCG.DOFade(1, 1);
        HUDManager.instance.missionRectTransform
            .DOAnchorPos(new Vector2(225.5f, HUDManager.instance.missionRectTransform.anchoredPosition.y), 1);
    }

    public void HideMission()
    {
        HUDManager.instance.missionRectTransform
            .DOAnchorPos(new Vector2(-325, HUDManager.instance.missionRectTransform.anchoredPosition.y), 1)
            .OnComplete(() =>
        {
            HUDManager.instance.missionRectTransform
                .DOAnchorPos(new Vector2(-325, HUDManager.instance.missionRectTransform.anchoredPosition.y), .5f)
                .OnComplete(() =>
            {
                if(missionIndex < missionSO.missions.Length - 1)
                {
                    missionIndex++;
                }
                DisplayMission();
            });
        });
    }

    #endregion
}
