using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SmokeInhalation : MonoBehaviour
{
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
}
