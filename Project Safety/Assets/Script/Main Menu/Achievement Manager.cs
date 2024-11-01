using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance {get; private set;}

    void Awake()
    {
        instance = this;
    }

    [Header("HUD")]
    public TMP_Text achievementTitleText;
    public Image achievementIcon;
    public TMP_Text achievementDescriptionText;
    
    [Space(5)]
    [SerializeField] Button dreamscape;
    [SerializeField] Button beenScolded;
    [SerializeField] Button triangleOfFire;
    [SerializeField] Button fireForce;
    [SerializeField] Button savingPrivateDummy;
    [SerializeField] Button dummynt;
    [SerializeField] Button justKeepBreathingDory;
    [SerializeField] Button whoYouGonnaCallFirebuster;
    [SerializeField] Button doNotDisturb;
    [SerializeField] Button anyPercent;
    [SerializeField] Button tooHotToHandle;
    [SerializeField] Button chainSmoker;
    [SerializeField] Button tpassMaster;
    [SerializeField] Button fireClassMaster;
    [SerializeField] Button fireExtinguishMaster;
    [SerializeField] Button fireSafetyMaster;
    
    [Space(5)]
    [SerializeField] Image dreamscapeImg;
    [SerializeField] Image beenScoldedImg;
    [SerializeField] Image triangleOfFireImg;
    [SerializeField] Image fireForceImg;
    [SerializeField] Image savingPrivateDummyImg;
    [SerializeField] Image dummyntImg;
    [SerializeField] Image justKeepBreathingDoryImg;
    [SerializeField] Image whoYouGonnaCallFirebusterImg;
    [SerializeField] Image doNotDisturbImg;
    [SerializeField] Image anyPercentImg;
    [SerializeField] Image tooHotToHandleImg;
    [SerializeField] Image chainSmokerImg;
    [SerializeField] Image tpassMasterImg;
    [SerializeField] Image fireClassMasterImg;
    [SerializeField] Image fireExtinguishMasterImg;
    [SerializeField] Image fireSafetyMasterImg;
    [Space(5)]
    [SerializeField] Sprite[] achievementStatus;
    [SerializeField] Sprite[] achievementSprite;

    void Start()
    {

        #region  Dreamscape

        FirebaseManager.Instance.GetAchievementStatusFromFirebase("Prologue - Dreamscape", (isUnlocked) =>
        {
            if (isUnlocked)
            {
                PlayerPrefs.SetInt("Prologue - Dreamscape", 1);
            }

            if (PlayerPrefs.GetInt("Prologue - Dreamscape", 0) == 1)
            {
                dreamscapeImg.sprite = achievementSprite[0];
                dreamscape.interactable = true;
                dreamscape.image.sprite = achievementStatus[1];

                if (!isUnlocked)
                {
                    FirebaseManager.Instance.SaveAchievementToFirebase("Prologue - Dreamscape", true);
                }
            }
            else
            {
                dreamscape.image.sprite = achievementStatus[0];
                dreamscape.interactable = false;
            }
        });

        #endregion

        #region  Been Scolded

        FirebaseManager.Instance.GetAchievementStatusFromFirebase("A1S1 - Been Scolded!", (isUnlocked) =>
        {
            if (isUnlocked)
            {
                PlayerPrefs.SetInt("A1S1 - Been Scolded!", 1);
            }

            if (PlayerPrefs.GetInt("A1S1 - Been Scolded!", 0) == 1)
            {
                beenScoldedImg.sprite = achievementSprite[1];
                beenScolded.interactable = true;
                beenScolded.image.sprite = achievementStatus[1];

                if (!isUnlocked)
                {
                    FirebaseManager.Instance.SaveAchievementToFirebase("PA1S1 - Been Scolded!", true);
                }
            }
            else
            {
                beenScolded.image.sprite = achievementStatus[0];
                beenScolded.interactable = false;
            }
        });

        #endregion

        #region Triangle of Fire

        FirebaseManager.Instance.GetAchievementStatusFromFirebase("A1S2 - Triangle of Fire", (isUnlocked) =>
        {
            if (isUnlocked)
            {
                PlayerPrefs.SetInt("A1S2 - Triangle of Fire", 1);
            }

            if (PlayerPrefs.GetInt("A1S2 - Triangle of Fire") == 1)
            {
            triangleOfFireImg.sprite = achievementSprite[2];
            triangleOfFire.interactable = true;
            triangleOfFire.image.sprite = achievementStatus[1];

                if (!isUnlocked)
                {
                    FirebaseManager.Instance.SaveAchievementToFirebase("A1S2 - Triangle of Fire", true);
                }
            }
            else
            {
            triangleOfFire.image.sprite = achievementStatus[0];
            triangleOfFire.interactable = false;
            }
        });

        #endregion

        #region Fire Force

        FirebaseManager.Instance.GetAchievementStatusFromFirebase("A1S3 - Fire Force", (isUnlocked) =>
        {
            if (isUnlocked)
            {
                PlayerPrefs.SetInt("A1S3 - Fire Force", 1);
            }

            if (PlayerPrefs.GetInt("A1S3 - Fire Force") == 1)
            {
            fireForceImg.sprite = achievementSprite[3];
            fireForce.interactable = true;
            fireForce.image.sprite = achievementStatus[1];

                if (!isUnlocked)
                {
                    FirebaseManager.Instance.SaveAchievementToFirebase("A1S3 - Fire Force", true);
                }
            }
            else
            {
            fireForce.image.sprite = achievementStatus[0];
            fireForce.interactable = false;
            }
        });

        #endregion

        #region Saving Private Dummy

        FirebaseManager.Instance.GetAchievementStatusFromFirebase("A1S4 - Saving Private Dummy", (isUnlocked) =>
        {
            if (isUnlocked)
            {
                PlayerPrefs.SetInt("A1S4 - Saving Private Dummy", 1);
            }

            if (PlayerPrefs.GetInt("A1S4 - Saving Private Dummy", 0) == 1)
            {
            savingPrivateDummyImg.sprite = achievementSprite[4];
            savingPrivateDummy.interactable = true;
            savingPrivateDummy.image.sprite = achievementStatus[1];

                if (!isUnlocked)
                {
                    FirebaseManager.Instance.SaveAchievementToFirebase("A1S4 - Saving Private Dummy", true);
                }
            }
            else
            {
            savingPrivateDummy.image.sprite = achievementStatus[0];
            savingPrivateDummy.interactable = false;
            }
        });

        #endregion

        #region  Dummn't

        FirebaseManager.Instance.GetAchievementStatusFromFirebase("A1S4 - Dummyn\'t", (isUnlocked) =>
        {
            if (isUnlocked)
            {
                PlayerPrefs.SetInt("A1S4 - Dummyn\'t", 1);
            }

            if (PlayerPrefs.GetInt("A1S4 - Dummyn\'t", 0) == 1)
            {
            dummyntImg.sprite = achievementSprite[5];
            dummynt.interactable = true;
            dummynt.image.sprite = achievementStatus[1];

                if (!isUnlocked)
                {
                    FirebaseManager.Instance.SaveAchievementToFirebase("A1S4 - Dummyn\'t", true);
                }
            }
            else
            {
            dummynt.image.sprite = achievementStatus[0];
            dummynt.interactable = false;
            }
        });

        #endregion

        #region  Just Keep Breating, Dory!

        FirebaseManager.Instance.GetAchievementStatusFromFirebase("A2S1 - Just Keep Breathing, Dory!", (isUnlocked) =>
        {
            if (isUnlocked)
            {
                PlayerPrefs.SetInt("A2S1 - Just Keep Breathing, Dory!", 1);
            }

            if (PlayerPrefs.GetInt("A2S1 - Just Keep Breathing, Dory!") == 1)
            {
            justKeepBreathingDoryImg.sprite = achievementSprite[6];
            justKeepBreathingDory.interactable = true;
            justKeepBreathingDory.image.sprite = achievementStatus[1];

                if (!isUnlocked)
                {
                    FirebaseManager.Instance.SaveAchievementToFirebase("A2S1 - Just Keep Breathing, Dory!", true);
                }
            }
            else
            {
            justKeepBreathingDory.image.sprite = achievementStatus[0];
            justKeepBreathingDory.interactable = false;
            }
        });

        #endregion

        #region Who You Gonna Call? FireBusters!!

        FirebaseManager.Instance.GetAchievementStatusFromFirebase("A2S1 - Who You Gonna Call? FireBusters!!", (isUnlocked) =>
        {
            if (isUnlocked)
            {
                PlayerPrefs.SetInt("A2S1 - Who You Gonna Call? FireBusters!!", 1);
            }

            if (PlayerPrefs.GetInt("A2S1 - Who You Gonna Call? FireBusters!!") == 1)
            {
            whoYouGonnaCallFirebusterImg.sprite = achievementSprite[7];
            whoYouGonnaCallFirebuster.interactable = true;
            whoYouGonnaCallFirebuster.image.sprite = achievementStatus[1];

                if (!isUnlocked)
                {
                    FirebaseManager.Instance.SaveAchievementToFirebase("A2S1 - Who You Gonna Call? FireBusters!!", true);
                }
            }
            else
            {
            whoYouGonnaCallFirebuster.image.sprite = achievementStatus[0];
            whoYouGonnaCallFirebuster.interactable = false;
            }
        });

        #endregion

        #region  Do not Disturb

        FirebaseManager.Instance.GetAchievementStatusFromFirebase("A2S1 - Do not Disturb?", (isUnlocked) =>
        {
            if (isUnlocked)
            {
                PlayerPrefs.SetInt("A2S1 - Do not Disturb?", 1);
            }

            if (PlayerPrefs.GetInt("A2S1 - Do not Disturb?") == 1)
            {
            doNotDisturbImg.sprite = achievementSprite[8];
            doNotDisturb.interactable = true;
            doNotDisturb.image.sprite = achievementStatus[1];

                if (!isUnlocked)
                {
                    FirebaseManager.Instance.SaveAchievementToFirebase("A2S1 - Do not Disturb?", true);
                }
            }
                else
            {
            doNotDisturb.image.sprite = achievementStatus[0];
            doNotDisturb.interactable = false;
            }
        });

        #endregion

        #region Any Percent

        FirebaseManager.Instance.GetAchievementStatusFromFirebase("A2S2 - Any%?", (isUnlocked) =>
        {
            if (isUnlocked)
            {
                PlayerPrefs.SetInt("A2S2 - Any%?", 1);
            }

            if (PlayerPrefs.GetInt("A2S2 - Any%?") == 1)
            {
            anyPercentImg.sprite = achievementSprite[9];
            anyPercent.interactable = true;
            anyPercent.image.sprite = achievementStatus[1];

                if (!isUnlocked)
                {
                    FirebaseManager.Instance.SaveAchievementToFirebase("A2S2 - Any%?", true);
                }
            }
            else
            {
            anyPercent.image.sprite = achievementStatus[0];
            anyPercent.interactable = false;
            }
        });

        #endregion

        #region Too Hot To Handle

        FirebaseManager.Instance.GetAchievementStatusFromFirebase("A2S2 - Too Hot To Handle", (isUnlocked) =>
        {
            if (isUnlocked)
            {
                PlayerPrefs.SetInt("A2S2 - Too Hot To Handle", 1);
            }

            if (PlayerPrefs.GetInt("A2S2 - Too Hot To Handle") == 1)
            {
            tooHotToHandleImg.sprite = achievementSprite[10];
            tooHotToHandle.interactable = true;
            tooHotToHandle.image.sprite = achievementStatus[1];

                if (!isUnlocked)
                {
                    FirebaseManager.Instance.SaveAchievementToFirebase("A2S2 - Too Hot To Handle", true);
                }
            }
            else
            {
            tooHotToHandle.image.sprite = achievementStatus[0];
            tooHotToHandle.interactable = false;
            }
        });

        #endregion

        #region  Chain Smoker

        FirebaseManager.Instance.GetAchievementStatusFromFirebase("A2S2 - Chain Smoker", (isUnlocked) =>
        {
            if (isUnlocked)
            {
                PlayerPrefs.SetInt("A2S2 - Chain Smoker", 1);
            }

            if (PlayerPrefs.GetInt("A2S2 - Chain Smoker") == 1)
            {
            chainSmokerImg.sprite = achievementSprite[11];
            chainSmoker.interactable = true;
            chainSmoker.image.sprite = achievementStatus[1];

                if (!isUnlocked)
                {
                    FirebaseManager.Instance.SaveAchievementToFirebase("A2S2 - Chain Smoker", true);
                }
            }
            else
            {
            chainSmoker.image.sprite = achievementStatus[0];
            chainSmoker.interactable = false;
            }
        });

        #endregion

        #region TPASS master

        FirebaseManager.Instance.GetAchievementStatusFromFirebase("Post Assessment - TPASS Master", (isUnlocked) =>
        {
            if (isUnlocked)
            {
                PlayerPrefs.SetInt("Post Assessment - TPASS Master", 1);
            }

            if (PlayerPrefs.GetInt("Post Assessment - TPASS Master") == 1)
        {
            tpassMasterImg.sprite = achievementSprite[12];
            tpassMaster.interactable = true;
            tpassMaster.image.sprite = achievementStatus[1];

                if (!isUnlocked)
                {
                    FirebaseManager.Instance.SaveAchievementToFirebase("Post Assessment - TPASS Master", true);
                }
            }
        else
        {
            tpassMaster.image.sprite = achievementStatus[0];
            tpassMaster.interactable = false;
        }
        });

        #endregion

        #region Fire Class Master

        FirebaseManager.Instance.GetAchievementStatusFromFirebase("Post Assessment - Fire Class Master", (isUnlocked) =>
        {
            if (isUnlocked)
            {
                PlayerPrefs.SetInt("Post Assessment - Fire Class Master", 1);
            }

            if (PlayerPrefs.GetInt("Post Assessment - Fire Class Master") == 1)
            {
            fireClassMasterImg.sprite = achievementSprite[13];
            fireClassMaster.interactable = true;
            fireClassMaster.image.sprite = achievementStatus[1];

                if (!isUnlocked)
                {
                    FirebaseManager.Instance.SaveAchievementToFirebase("Post Assessment - Fire Class Master", true);
                }
            }
            else
            {
            fireClassMaster.image.sprite = achievementStatus[0];
            fireClassMaster.interactable = false;
            }
        });

        #endregion

        #region Fire Extinguisher Master

        FirebaseManager.Instance.GetAchievementStatusFromFirebase("Post Assessment - Fire Extinguisher Master", (isUnlocked) =>
        {
            if (isUnlocked)
            {
                PlayerPrefs.SetInt("Post Assessment - Fire Extinguisher Master", 1);
            }

            if (PlayerPrefs.GetInt("Post Assessment - Fire Extinguisher Master") == 1)
            {
            fireExtinguishMasterImg.sprite = achievementSprite[14];
            fireExtinguishMaster.interactable = true;
            fireExtinguishMaster.interactable = achievementStatus[1];

                if (!isUnlocked)
                {
                    FirebaseManager.Instance.SaveAchievementToFirebase("Post Assessment - Fire Extinguisher Master", true);
                }

            }
            else
            {
            fireExtinguishMaster.image.sprite = achievementStatus[0];
            fireExtinguishMaster.interactable = false;
            }
        });

        #endregion

        #region Fire Safety Master

        FirebaseManager.Instance.GetAchievementStatusFromFirebase("Post Assessment - Extinguish Master", (isUnlocked) =>
        {
            if (isUnlocked)
            {
                PlayerPrefs.SetInt("Post Assessment - Extinguish Master", 1);
            }

            if (PlayerPrefs.GetInt("Post Assessment - Extinguish Master") == 1)
            {
            fireSafetyMasterImg.sprite = achievementSprite[15];
            fireSafetyMaster.interactable = true;
            fireSafetyMaster.image.sprite = achievementStatus[1];

                if (!isUnlocked)
                {
                    FirebaseManager.Instance.SaveAchievementToFirebase("Post Assessment - Extinguish Master", true);
                }
            }
            else
            {
            fireSafetyMaster.image.sprite = achievementStatus[0];
            fireSafetyMaster.interactable = false;
            }
        });

        #endregion
    }

    public void DisplayAchievementDescription(AchievementSO achievementSO)
    {
        achievementTitleText.text = achievementSO.achievementName;
        achievementDescriptionText.text = achievementSO.achievementDescription;
        achievementIcon.sprite = achievementSO.achievementSprite;   
    }
}




