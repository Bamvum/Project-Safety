using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MissionManager : MonoBehaviour
{
    public static MissionManager instance {get; private set;}

    void Awake()
    {
        instance = this;
    }   

    [SerializeField] MissionSO missionSO;
    
    [Space(5)]
    public TMP_Text missionText;
    public RectTransform missionRectTransform;
    public CanvasGroup missionCG;
    
    [Space(10)]
    public AudioSource missionSFX;
    int missionIndex;

    void Start()
    {
        MissionPropertiesReset();
    }

    void MissionPropertiesReset()
    {
        // ASSIGN
        missionRectTransform.anchoredPosition = new Vector2(-325, missionRectTransform.anchoredPosition.y);
        missionCG.alpha = 0;
    }
    public void DisplayMission()
    {
        missionText.text = missionSO.missions[missionIndex];
        missionSFX.Play();

        missionCG.DOFade(1, 1);
        missionRectTransform.DOAnchorPos(new Vector2(225.5f, missionRectTransform.anchoredPosition.y), 1);
    
        if(SceneManager.GetActiveScene().name == "Prologue")
        {
            if (missionIndex == 1)
            {
                PrologueSceneManager.instance.PC.layer = 8;
            }
            else
            {
                PrologueSceneManager.instance.PC.layer = 0;
            }
        }
    }

    public void HideMission()
    {
        missionRectTransform
            .DOAnchorPos(new Vector2(-325, missionRectTransform.anchoredPosition.y), 1)
            .OnComplete(() =>
        {
            missionRectTransform.DOAnchorPos(new Vector2(-325, missionRectTransform.anchoredPosition.y), .5f)
                .OnComplete(() =>
            {
                if (missionIndex < missionSO.missions.Length - 1)
                {
                    missionIndex++;
                }
                DisplayMission();
            });
        });
    }
}
