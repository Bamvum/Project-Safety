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
        // CHECK IF ACHIEVEMENT IS ALREADY UNLOCK (IF ACHIEVEMENET IS NOT UNLOCK CODE EXCUTE BELOW)
        if(PlayerPrefs.GetInt(achievementSO.achievementPlayerPrefsKey) == 0)
        {
            achievementName.text = achievementSO.achievementName;
            achievementImage.sprite = achievementSO.achievementSprite;

            // ACHIEVEMENT STORE IN PLAYER PREFS
            PlayerPrefs.SetInt(achievementSO.achievementPlayerPrefsKey, 1);

            achievementSFX.Play();
            achievementRectTransform.DOAnchorPos(new Vector2(achievementRectTransform.anchoredPosition.x, 425), 1)
                .SetEase(Ease.OutQuad)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    achievementRectTransform.DOAnchorPos(new Vector2(achievementRectTransform.anchoredPosition.x, 675), 1)
                        .SetEase(Ease.OutQuad)
                        .SetUpdate(true)
                        .SetDelay(5);
                });
        }

    }
}
