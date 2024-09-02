using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Act1Scene3SceneManager : MonoBehaviour
{
    public static Act1Scene3SceneManager instance { get; private set;} 
    // Update is called once per frame
    
    void Awake()
    {
        instance = this;
    }
    [SerializeField] GameObject fireTruck;

    [Header("Flags")] 
    [SerializeField] float fireTruckSpeed;
    [SerializeField] bool moveTowardFireTruck;
    
    void Start()
    {
        // // FADE IMAGE ALPHA SET 1
        // LoadingSceneManager.instance.fadeImage.color = new Color(LoadingSceneManager.instance.fadeImage.color.r,
        //                                                         LoadingSceneManager.instance.fadeImage.color.g,
        //                                                         LoadingSceneManager.instance.fadeImage.color.b,
        //                                                         1);

    }

    void Update()
    {
        if(!moveTowardFireTruck)
        {
            fireTruck.transform.position = Vector3.Lerp(fireTruck.transform.position, new Vector3 (fireTruck.transform.position.x, fireTruck.transform.position.y, -21), Time.deltaTime * fireTruckSpeed);

            if(fireTruck.transform.position == new Vector3(fireTruck.transform.position.x, fireTruck.transform.position.y, -21))
            {
                moveTowardFireTruck = true;

                Debug.Log("Start Dialogue Trigger!");
            }
        }
    }
}
