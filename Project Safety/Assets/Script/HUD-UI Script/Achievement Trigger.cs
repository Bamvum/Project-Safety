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
        // CHECK IF ACHIEVEMENT IS ALREADY UNLOCKED (IF ACHIEVEMENT IS NOT UNLOCKED, CODE EXECUTES BELOW)
        if (PlayerPrefs.GetInt(achievementSO.achievementPlayerPrefsKey) == 0)
        {
            achievementName.text = achievementSO.achievementName;
            achievementImage.sprite = achievementSO.achievementSprite;

            // ACHIEVEMENT STORE IN PLAYER PREFS
            PlayerPrefs.SetInt(achievementSO.achievementPlayerPrefsKey, 1);

            // SAVE ACHIEVEMENT TO FIREBASE
            FirebaseManager.Instance.SaveAchievementToFirebase(achievementSO.achievementPlayerPrefsKey, true);

            achievementSFX.Play();
            achievementRectTransform.DOAnchorPos(new Vector2(achievementRectTransform.anchoredPosition.x, -200), 1)
                .SetEase(Ease.OutQuad)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    achievementRectTransform.DOAnchorPos(new Vector2(achievementRectTransform.anchoredPosition.x, 0), 1)
                        .SetEase(Ease.OutQuad)
                        .SetUpdate(true)
                        .SetDelay(5);
                });
        }
    }
}
