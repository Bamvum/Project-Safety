using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class PrologueSceneManager : MonoBehaviour
{
    public static PrologueSceneManager instance {get; private set;}

    void Awake()
    {
        instance = this;
    }
 
    [Header("Script")]
    public OnAndOffGameObject onAndOffGameObject;

    [Header("Player")]
    [SerializeField] GameObject playerModel;
    
    [Space(10)]
    [SerializeField] AudioSource suspenceSFX;
    public AudioSource alarmSFX;

    [Header("Prologue Game Object")]
    public GameObject PC;
    public GameObject monitor;
    public GameObject[] monitorScreen;
    public GameObject lightSwitch;

    [Header("Dialogue Triggers")]
    [SerializeField] DialogueTrigger startDialogueTrigger;
    
    [Space(15)]
    public AudioSource monitorSFX;

    [Header("Flag")]
    public bool toGetUp;
    bool isSuspenceSFXPlaying;

    [SerializeField] Volume globalVolume; 
    Vignette vignette;

    void Start()
    {
        // TODO -   IF PAUSE UI IS ACTIVE
        //      -   AND IF STATEMENT DEVICEMANAGER

        // Cursor.lockState = CursorLockMode.Locked;

        Cursor.lockState = CursorLockMode.None;  // Example: Unlock the cursor

        // FADE IMAGE ALPHA SET 1
        LoadingSceneManager.instance.fadeImage.color = new Color(LoadingSceneManager.instance.fadeImage.color.r,
                                                                LoadingSceneManager.instance.fadeImage.color.g,
                                                                LoadingSceneManager.instance.fadeImage.color.b,
                                                                1);

        // globalVolume.profile.TryGet(out vignette);

        StartCoroutine(FadeOutFadeImage());
    }
    
    void Update()
    {
        // TO NEXT SCENE (ACT 1 - STUDENT)
        if(isSuspenceSFXPlaying)
        {
            Debug.Log("Suspence SFX is Playing!");

            // ELAPSED TIME == suspenseSFX.Clip.length
            if(suspenceSFX.time >= suspenceSFX.clip.length)
            {
                LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);

                LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                {
                    PlayerScript.instance.DisablePlayerScripts();

                    LoadingSceneManager.instance.loadingScreen.SetActive(true);
                    LoadingSceneManager.instance.enabled = true;
                    // NEXT SCENE NAME
                    LoadingSceneManager.instance.sceneName = "Act 1 Scene 1";
                });

                isSuspenceSFXPlaying = false;
            }    
        }
    }

    IEnumerator FadeOutFadeImage()
    {
        yield return new WaitForSeconds(2);
        alarmSFX.Play();

        yield return new WaitForSeconds(5);
        Debug.Log("Wait for 5 Seconds");
        LoadingSceneManager.instance.fadeImage
                .DOFade(0, LoadingSceneManager.instance.fadeDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
        {
           
            LoadingSceneManager.instance.fadeImage.gameObject.SetActive(false);
        });

        yield return new WaitForSeconds(3);
        toGetUp = true;
    }


    public void TransitionToHomeworkQuiz()
    {
        LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);
        LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
        {
            HomeworkManager.instance.enabled = true;
            HomeworkManager.instance.homeworkHUD.SetActive(true);
            LoadingSceneManager.instance.fadeImage.DOFade(0, LoadingSceneManager.instance.fadeDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() => 
            {
                LoadingSceneManager.instance.fadeImage.gameObject.SetActive(false);
                HomeworkManager.instance.homeworkHUD.SetActive(true);
                PlayerScript.instance.playerMovement.enabled = false;
                PlayerScript.instance.cinemachineInputProvider.enabled = false;
                PlayerScript.instance.stamina.enabled = false;
            });
        });
    }

    #region - SCENE MANAGEMENT -

    [ContextMenu("Rotate")]
    public void RotatePlayer()
    {
        // CAMAERA FIX (MAKE IT FACE A CERTAIN AXIS)
        playerModel.transform.rotation = Quaternion.Euler(0, 120,0);
    }

    [ContextMenu("Move")]
    public void MovePlayer()
    {
        playerModel.transform.position = new Vector3(-6.5f, playerModel.transform.position.y, -11);
        // playerModel.transform.position = new Vector3(0,0,0);
    }

    public void StartSuspenceSequence()
    {
        //TODO  -   CHANGE ALL LAYER OF EXAMINABLE GAMEOBJECT TO DEFAULT (LAYER 0)
        //      -   START SUSPENCE SOUND
        //      -   START VIGNETTE

        suspenceSFX.Play();
        isSuspenceSFXPlaying = true; 

        // vignette.intensity.
    }

    #endregion
}
