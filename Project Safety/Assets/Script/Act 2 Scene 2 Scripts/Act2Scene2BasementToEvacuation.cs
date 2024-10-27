using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Act2Scene2BasementToEvacuation : MonoBehaviour
{
    [Header("DialogueTrigger")]
    [SerializeField] DialogueTrigger evacuationDialogueEng;
    [SerializeField] DialogueTrigger evacuationDialogueTag;

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
                    // TELEPORT PLAYER
                    PlayerScript.instance.playerMovement.gameObject.transform.position = new Vector3(8.5f, 0, 87);
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

                                    if (Act2Scene2SceneManager.instance.languageIndex == 0)
                                    {
                                        evacuationDialogueEng.StartDialogue();
                                    }
                                    else if (Act2Scene2SceneManager.instance.languageIndex == 1)
                                    {
                                        evacuationDialogueTag.StartDialogue();
                                    }
                                });
                        });
                });
        }
    }
}
