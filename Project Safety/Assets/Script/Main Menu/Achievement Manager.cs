using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{

    [Header("Achievements")]
    [SerializeField] Button beenScolded;
    [SerializeField] Button savingPrivateDummy;
    [SerializeField] Button dummynt;
    [SerializeField] Button justKeepBreathingDory;
    [SerializeField] Button whoYouGonnaCallFirebuster;
    [SerializeField] Button doNotDisturb;
    [SerializeField] Button anyPercent;
    [SerializeField] Button tooHotToHandle;
    [SerializeField] Button chainSmoker;
    
    [SerializeField] Sprite[] achievementStatus;
    [SerializeField] Sprite[] achievementSprite;

    void Start()
    {
        #region  Been Scolded

        if (PlayerPrefs.GetInt("A1S1 - Been Scolded!", 0) == 1)
        {
            Image achievementImg = beenScolded.GetComponentInChildren<Image>();
            beenScolded.interactable = true;
            beenScolded.image.sprite = achievementStatus[1];
        }
        else
        {
            Debug.Log("A1S4 - Saving Private Dummy is Lock");
            beenScolded.image.sprite = achievementStatus[0];
            beenScolded.interactable = false;
        }

        #endregion

        #region Saving Private Dummy

        if (PlayerPrefs.GetInt("A1S4 - Saving Private Dummy", 0) == 1)
        {
            Image achievementImg = savingPrivateDummy.GetComponentInChildren<Image>();
            savingPrivateDummy.interactable = true;
            savingPrivateDummy.image.sprite = achievementStatus[1];

            Debug.Log("A1S4 - Saving Private Dummy is Unlock");
        }
        else
        {
            Debug.Log("A1S4 - Saving Private Dummy is Lock");
            savingPrivateDummy.image.sprite = achievementStatus[0];
            savingPrivateDummy.interactable = false;
        }

        #endregion

        #region  Dummn't

        if (PlayerPrefs.GetInt("A1S4 - Dummyn\'t", 0) == 1)
        {
            Image achievementImg = dummynt.GetComponentInChildren<Image>(); // CHANGE "?" TO "Achievement Image"
            dummynt.interactable = true;
            dummynt.image.sprite = achievementStatus[1];
        }
        else
        {
            dummynt.image.sprite = achievementStatus[0];
            dummynt.interactable = false;
        }

        #endregion

        #region  Just Keep Breating, Dory!
        
        if (PlayerPrefs.GetInt("A2S1 - Just Keep Breating, Dory!") == 1)
        {
            Image achievementImg = justKeepBreathingDory.GetComponentInChildren<Image>(); // CHANGE "?" TO "Achievement Image"
            justKeepBreathingDory.interactable = true;
            justKeepBreathingDory.image.sprite = achievementStatus[1];
        }
        else
        {
            justKeepBreathingDory.image.sprite = achievementStatus[0];
            justKeepBreathingDory.interactable = false;
        }

        #endregion

        #region Who You Gonna Call? FireBusters!!

        if (PlayerPrefs.GetInt("A2S1 - Who You Gonna Call? FireBusters!!") == 1)
        {
            Image achievementImg = whoYouGonnaCallFirebuster.GetComponentInChildren<Image>(); // CHANGE "?" TO "Achievement Image"
            whoYouGonnaCallFirebuster.interactable = true;
            whoYouGonnaCallFirebuster.image.sprite = achievementStatus[1];
        }
        else
        {
            whoYouGonnaCallFirebuster.image.sprite = achievementStatus[0];
            whoYouGonnaCallFirebuster.interactable = false;
        }

        #endregion


        #region  Do not Disturb

        if (PlayerPrefs.GetInt("A2S1 - Do not Disturb?") == 1)
        {
            Image achievementImg = doNotDisturb.GetComponentInChildren<Image>(); // CHANGE "?" TO "Achievement Image"
            doNotDisturb.interactable = true;
            doNotDisturb.image.sprite = achievementStatus[1];
        }
        else
        {
            doNotDisturb.image.sprite = achievementStatus[0];
            doNotDisturb.interactable = false;
        }

        #endregion

        #region Any Percent

        if (PlayerPrefs.GetInt("A2S2 - Any%?") == 1)
        {
            Image achievementImg = anyPercent.GetComponentInChildren<Image>(); // CHANGE "?" TO "Achievement Image"
            anyPercent.interactable = true;
            anyPercent.image.sprite = achievementStatus[1];
        }
        else
        {
            anyPercent.image.sprite = achievementStatus[0];
            anyPercent.interactable = false;
        }

        #endregion

        #region Too Hot To Handle

        if (PlayerPrefs.GetInt("A2S2 - Too Hot To Handle") == 1)
        {
            Image achievementImg = tooHotToHandle.GetComponentInChildren<Image>(); // CHANGE "?" TO "Achievement Image"
            tooHotToHandle.interactable = true;
            tooHotToHandle.image.sprite = achievementStatus[1];
        }
        else
        {
            tooHotToHandle.image.sprite = achievementStatus[0];
            tooHotToHandle.interactable = false;
        }

        #endregion

        #region  Chain Smoker

        if (PlayerPrefs.GetInt("A2S2 - Chain Smoker") == 1)
        {
            Image achievementImg = chainSmoker.GetComponentInChildren<Image>(); // CHANGE "?" TO "Achievement Image"
            chainSmoker.interactable = true;
            chainSmoker.image.sprite = achievementStatus[1];
        }
        else
        {
            chainSmoker.image.sprite = achievementStatus[0];
            chainSmoker.interactable = false;
        }

        #endregion
    }
}


// PlayerPrefs.SetInt("Key Name", 1);

/*
A1S1 Television - 

Been Scolded - Choose not to unplug the plugs

A1S4 Dummy:

Saving Private Dummy - Choose to save the Dummy

Dummyn’t - Choose to not save the Dummy


A2S1 Stay Calm:

Just Keep Breathing, Dory! - Complete Stay Calm

A2S1 Contact:

Who you gonna call? Firebusters!! - Local Fire Station and National Emergency Hotline

Do not Disturb? - Mom’s number

A2S2 Escape:

Any%? - Escape under 3 minutes

A2S2 Check Door:

Too Hot to Handle - Try to open a burning door 2 times.

A2S2 Smoke Inhalation:

Chainsmoker - 80% Lungs is filled with smoke
*/

/*

#region Data Types

    #region Achievements
    [Header("Achievements")]
    [SerializeField] private GameObject weStartOffSomewhereAchievement;
    [SerializeField] private GameObject bravoAchievement;
    #endregion  
    
    #region Achievements Checker

    [Header("Achievements Checker")]
    [SerializeField] public bool isAID1Unlocked;
    [SerializeField] public bool isAID2Unlocked;
    #endregion
    
#endregion

#region Start

    // Start is called before the first frame update
    void Start()
    {
#region Checking of Null
        if(SceneManager.GetActiveScene().name != "0 - Lobby")
        {
            if(weStartOffSomewhereAchievement == null)
            {
                return;
            }

            if(bravoAchievement == null)
            {
                return;
            }
        }
#endregion

#region PlayerPrefs
        // We Start Off Somewhere Achievement
        if(PlayerPrefs.GetInt("We Start Off Somewhere Achievement", 0) == 1)    
        {
            weStartOffSomewhereAchievement.SetActive(true);
            isAID1Unlocked = true;
        }
        
        // Bravo
        if (PlayerPrefs.GetInt("Bravo Achievement", 0) == 1)
        {
            bravoAchievement.SetActive(true);
            isAID2Unlocked = true;
        }
#endregion
    
    } 
#endregion  

*/