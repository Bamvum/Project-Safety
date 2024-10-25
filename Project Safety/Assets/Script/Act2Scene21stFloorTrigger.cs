using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Act2Scene21stFloorTrigger : MonoBehaviour
{

    [SerializeField] GameObject firstFloor;
    [SerializeField] GameObject groundFloor;
    [SerializeField] GameObject basement;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Pause.instance.canInput = false;
            PlayerScript.instance.DisablePlayerScripts();
            
            LoadingSceneManager.instance.fadeImage.gameObject.SetActive(true);
            LoadingSceneManager.instance.fadeImage.DOFade(1, LoadingSceneManager.instance.fadeDuration)
                .SetUpdate(true)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    firstFloor.SetActive(false);
                    groundFloor.SetActive(true);
                    basement.SetActive(true);

                    // TELEPORT PLAYER
                    PlayerScript.instance.playerMovement.gameObject.transform.position = new Vector3(80, 0, 48);
                    PlayerScript.instance.playerMovement.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
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

                                    Pause.instance.canInput = true;
                                });
                        });
                });
        }
    }
}
