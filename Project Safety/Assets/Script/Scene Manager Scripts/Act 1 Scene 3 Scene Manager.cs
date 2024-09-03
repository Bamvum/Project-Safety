using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Act1Scene3SceneManager : MonoBehaviour
{
    public static Act1Scene3SceneManager instance { get; private set;} 
    // Update is called once per frame
    
    void Awake()
    {
        instance = this;
    }

    [Header("Dialogue Trigger")]
    [SerializeField] DialogueTrigger startDialogue;

    [Header("Player")]
    [SerializeField] GameObject player;

    [Space(10)]
    [SerializeField] GameObject fireFighter;
    [SerializeField] GameObject fireTruckPeople;
    [SerializeField] GameObject fireTruck;

    [Space(10)]
    [SerializeField] AudioSource streetAmbiance;

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

        // TODO -   MONOLOGUE BY PLAYER
        //      -   

    }

    void Update()
    {
        if(!moveTowardFireTruck)
        {
            fireTruck.transform.position = Vector3.MoveTowards(fireTruck.transform.position, new Vector3 (fireTruck.transform.position.x, fireTruck.transform.position.y, -21), Time.deltaTime * fireTruckSpeed);

            if(fireTruck.transform.position == new Vector3(fireTruck.transform.position.x, fireTruck.transform.position.y, -21))
            {
                // FADE IN 
                HUDManager.instance.fadeImageForDialogue.gameObject.SetActive(true);
                HUDManager.instance.fadeImageForDialogue.DOFade(1, 2).OnComplete(() =>
                {
                    player.SetActive(true);
                    fireFighter.SetActive(true);
                    fireTruckPeople.SetActive(false);
                    HUDManager.instance.fadeImageForDialogue.DOFade(0, 2).OnComplete(() =>
                    {
                        HUDManager.instance.fadeImageForDialogue.gameObject.SetActive(true);
                        startDialogue.StartDialogue();

                        streetAmbiance.Play();
                    });
                });

                moveTowardFireTruck = true;
                

                
            }
        }
    }
}
