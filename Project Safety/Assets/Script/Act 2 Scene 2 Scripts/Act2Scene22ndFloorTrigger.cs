using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Act2Scene22ndFloorTrigger : MonoBehaviour
{
    [SerializeField] GameObject secondFloor;
    [SerializeField] GameObject firstFloor;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Pause.instance.PauseCanInput(false);
            PlayerScript.instance.DisablePlayerScripts();
            
            LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);
            LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
                .SetUpdate(true)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    secondFloor.SetActive(false);
                    firstFloor.SetActive(true);
                    
                    // TELEPORT PLAYER
                    PlayerScript.instance.playerMovement.gameObject.transform.position = new Vector3(22.5f, 3, 0);
                    PlayerScript.instance.playerMovement.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                    LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
                        .SetUpdate(true)
                        .OnComplete(() =>
                        {
                            LoadingSceneManager.instance.fadeImage.DOFade(0, LoadingSceneManager.instance.fadeDuration)
                                .SetUpdate(true)
                                .SetEase(Ease.Linear)
                                .OnComplete(() =>
                                {
                                    LoadingSceneManager.instance.fadeImage.gameObject.SetActive(false);

                                    PlayerScript.instance.playerMovement.enabled = true;
                                    PlayerScript.instance.cinemachineInputProvider.enabled = true;
                                    PlayerScript.instance.interact.enabled = true;
                                    PlayerScript.instance.stamina.enabled = true;

                                    Pause.instance.PauseCanInput(true);
                                });
                        });
                });
        }
    }
}
