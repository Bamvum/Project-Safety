using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AchievementTrigger : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] RectTransform achievementRectTransform;
    [SerializeField] TMP_Text achievementName;
    [SerializeField] Image achievementImage;
    [SerializeField] AudioSource achievementSFX;

    public void ShowAchievement(AchievementSO achievementSO)
    {
        achievementName.text = achievementSO.achievementName;
        achievementImage.sprite = achievementSO.achievementSprite;

        achievementSFX.Play();
        achievementRectTransform.DOAnchorPos(new Vector2(achievementRectTransform.anchoredPosition.x, 425), 1)
            .SetEase(Ease.OutQuad)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                PlayerPrefs.SetInt(achievementSO.achievementPlayerPrefsKey, 1);
                //yield for 1 seconds

                achievementRectTransform.DOAnchorPos(new Vector2(achievementRectTransform.anchoredPosition.x, 675), 1)
                    .SetEase(Ease.OutQuad)
                    .SetUpdate(true);
            });
    }
}
