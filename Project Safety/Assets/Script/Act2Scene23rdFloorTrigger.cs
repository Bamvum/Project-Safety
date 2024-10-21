using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Act2Scene23rdFloorTrigger : MonoBehaviour
{
    [SerializeField] bool stopRepeat;

    [SerializeField] GameObject showGameObject;
    void OnTriggerEnter(Collider other)
    {
        if(!stopRepeat)
        {
            if(other.gameObject.CompareTag("Player"))
            {
                showGameObject.SetActive(true);
                stopRepeat = true;
            }
        }
    }
}
