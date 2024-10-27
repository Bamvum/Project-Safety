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
            // DAMAGE OVERTIME
            // DECREASE MAXIMUM PLAYER STAMINA
            // HEALTH

            Debug.Log("Player inside Smoke");
            Act2Scene2SceneManager.instance.playerHealth -= .5f;
        }
    }
}
