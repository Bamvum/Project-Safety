using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;
using System.IO;

public class MissionManager : MonoBehaviour
{
    public static MissionManager instance {get; private set;}

    void Awake()
    {
        instance = this;
    }   

    public MissionSO missionSO;
    public MissionSO tagalogMissionSO;

    [SerializeField] int missionLanguageIndex;
    
    [Space(5)]
    public TMP_Text missionText;
    public RectTransform missionTextRectTransform;
    public CanvasGroup missionCG;
    
    [Space(10)]
    public AudioSource missionSFX;
    public int missionIndex;

    void Start()
    {
    }

    [ContextMenu("Display Mission")]
    public void DisplayMission()
    {
        if(SettingMenu.instance.languageDropdown.value == 0)
        {
            Debug.LogWarning("English Mission");
            missionText.text = missionSO.missions[missionIndex];
        }
        else
        {
            Debug.LogWarning("Tagalog Mission");
            missionText.text = tagalogMissionSO.missions[missionIndex];
        }

        missionSFX.Play();

        Sequence sequence = DOTween.Sequence();

        sequence.Append(missionCG.DOFade(1,1));
        sequence.AppendInterval(2);
        sequence.Append(missionTextRectTransform.DOAnchorPos(new Vector3(0, 50, 0), 1));
        sequence.Join(missionTextRectTransform.DOScale(Vector3.one, 1));

    
        if(SceneManager.GetActiveScene().name == "Prologue")
        {
            if (missionIndex == 2)
            {
                PrologueSceneManager.instance.PC.layer = 8;
            }
            else
            {
                PrologueSceneManager.instance.PC.layer = 0;
            }
        }
    }

    [ContextMenu("Hide")]
    public void HideMission()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(missionCG.DOFade(0, 1));
        sequence.Append(missionTextRectTransform.DOScale(new Vector3(2, 2, 2), 1));
        sequence.Join(missionTextRectTransform.DOAnchorPos(new Vector3(0, -540, 0), 1)).OnComplete(() =>
        {
            if (missionIndex < missionSO.missions.Length - 1)
            {
                missionIndex++;
            }

            DisplayMission();
        });
    }

}
