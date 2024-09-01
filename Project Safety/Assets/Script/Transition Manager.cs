using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineBlendTest : MonoBehaviour
{
    [SerializeField] CinemachineBrain cinemachineBrain;
    [SerializeField] CinemachineVirtualCamera virtualCamera1;
    [SerializeField] CinemachineVirtualCamera virtualCamera2;
    
    
    void Start()
    {
        // Get the CinemachineBrain component from the main camera
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
    }

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Space))
        {
            virtualCamera1.Priority = 0;
            virtualCamera2.Priority = 10;
        }
        // else
        // {
        //     virtualCamera1.Priority = 10;
        //     virtualCamera2.Priority = 0;
        // }


        // Check if a blend is currently happening
        if (cinemachineBrain.IsBlending)
        {
            Debug.Log("Cinemachine blend is in progress.");
        }
        else
        {
            Debug.Log("Cinemachine blend is complete.");
        }

    }
}
