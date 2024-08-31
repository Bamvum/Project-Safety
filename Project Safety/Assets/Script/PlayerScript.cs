using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript instance {get; private set;}

    void Awake()
    {
        instance = this;
    
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        playerControls = new PlayerControls();
    }

    public PlayerControls playerControls;

    [Header("Player")]
    public PlayerMovement playerMovement;
    [Space(10)]
    public CinemachineVirtualCamera playerVC;
    public CinemachineBrain cinemachineBrain;
    public CinemachineInputProvider cinemachineInputProvider;
    public Interact interact;
    public Examine examine;
    public Stamina stamina;

    [SerializeField] float playerRotationSpeed;
    

    public void DisablePlayerScripts()
    {
        playerMovement.enabled = false;
        cinemachineInputProvider.enabled = false;
        interact.enabled = false;
        examine.enabled = false;
        stamina.enabled = false;
    }

    public void RotatePlayerTowards(Transform LookAtObject)
    {
        StartCoroutine(StartRotatePlayer(LookAtObject));
    }

    IEnumerator StartRotatePlayer(Transform LookAtObject)
    {
        Vector3 direction = LookAtObject.position - playerMovement.transform.position;
        direction.y = 0; // Optional: Keep rotation in the horizontal plane only

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        while (playerMovement.transform.rotation != targetRotation)
        {                                                                                                                   // rotationSpeed
            playerMovement.transform.rotation = Quaternion.RotateTowards(playerMovement.transform.rotation, targetRotation, playerRotationSpeed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }
    }
    

}
