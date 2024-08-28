using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MissionManager : MonoBehaviour
{
    public static MissionManager instance {get; private set;}

    void Awake()
    {
        instance = this;
    }   

    [SerializeField] MissionSO missionSO;
    
    [Space(10)]
    public AudioSource missionSFX;
    int missionIndex;

    public void DisplayMission()
    {
        HUDManager.instance.missionText.text = missionSO.missions[missionIndex];
        missionSFX.Play();

        HUDManager.instance.missionCG.DOFade(1, 1);
        HUDManager.instance.missionRectTransform
            .DOAnchorPos(new Vector2(225.5f, HUDManager.instance.missionRectTransform.anchoredPosition.y), 1);
    
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
        HUDManager.instance.missionRectTransform
            .DOAnchorPos(new Vector2(-325, HUDManager.instance.missionRectTransform.anchoredPosition.y), 1)
            .OnComplete(() =>
        {
            HUDManager.instance.missionRectTransform
                .DOAnchorPos(new Vector2(-325, HUDManager.instance.missionRectTransform.anchoredPosition.y), .5f)
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
