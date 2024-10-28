using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{

    [Header("Achievements")]
    [SerializeField] Button savingPrivateDummy;
    [SerializeField] Button dummynt;
    [SerializeField] Button justKeepBreathingDory;
    [SerializeField] Button whoYouGonnaCallFirebuster;
    [SerializeField] Button doNotDisturb;
    [SerializeField] Button anyPercent;
    [SerializeField] Button tooHotToHandle;
    [SerializeField] Button chainSmoker;
    [SerializeField] Button test;
    
    [SerializeField] Sprite[] achievementStatus;
    [SerializeField] Sprite[] achievementSprite;

    void Start()
    {
        if(PlayerPrefs.GetInt("A1S4 - Saving Private Dummy", 0) == 1)
        {
            Image achievementImg = savingPrivateDummy.GetComponentInChildren<Image>();
            savingPrivateDummy.interactable = true;
            savingPrivateDummy.image.sprite =achievementStatus[0];
        }
        else
        {
            savingPrivateDummy.interactable = false;
        }
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