using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SmokeInhalation : MonoBehaviour
{
    [SerializeField] AudioSource coughSFX;
    [SerializeField] AudioSource hurtSFX;

    [SerializeField] bool isFire;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isFire)
            {
                hurtSFX.Play();
                hurtSFX.DOFade(1, 1).SetUpdate(true);
            }
            else
            {
                coughSFX.Play();
                coughSFX.DOFade(1, 1).SetUpdate(true);
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(!GameOver.instance.gameOverHUDCG.gameObject.activeSelf)
            {
                if (isFire)
                {
                    Debug.Log("Player inside Fire");
                    Act2Scene2SceneManager.instance.playerHealth -= .55f;
                }
                else
                {
                    Debug.Log("Player inside Smoke");
                    Act2Scene2SceneManager.instance.playerHealth -= .4f;
                }

            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isFire)
            {
                hurtSFX.DOFade(0, 1).SetUpdate(true);
            }
            else
            {
                coughSFX.DOFade(0, 1).SetUpdate(true);
            }
        }
    }

}

// Gamepad Vibrate

