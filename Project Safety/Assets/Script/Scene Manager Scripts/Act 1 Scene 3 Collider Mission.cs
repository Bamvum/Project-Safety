using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Act1Scene3ColliderMission : MonoBehaviour
{
    bool occurOnce;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !occurOnce)
        {
            MissionManager.instance.HideMission();
            occurOnce = true;
        }
    }
}
