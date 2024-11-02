using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SmokeInhalation : MonoBehaviour
{
    [SerializeField] AudioSource coughSFX;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            coughSFX.Play();
            coughSFX.DOFade(1, 1).SetUpdate(true);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(!GameOver.instance.gameOverHUDCG.gameObject.activeSelf)
            {
                Debug.Log("Player inside Smoke");
                Act2Scene2SceneManager.instance.playerHealth -= .5f;

            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            coughSFX.DOFade(0, 1).SetUpdate(true);
        }
    }

}

// Gamepad Vibrate

