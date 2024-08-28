using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InstructionManager : MonoBehaviour
{
    public static InstructionManager instance {get; private set;}

    void Awake()
    {
        instance = this;
    }

    [SerializeField] InstructionSO instructionSO;

    public void DisplayInstruction()
    {
        Time.timeScale = 0;
        HUDManager.instance.instructionHUD.SetActive(true);

        HUDManager.instance.instructionBGRectTransform
            .DOSizeDelta(new Vector2(1920, HUDManager.instance.instructionBGRectTransform.sizeDelta.y), .5f)
            .SetEase(Ease.InQuad)
            .SetUpdate(true)
            .OnComplete(() =>
        {
            HUDManager.instance.instructionContent.SetActive(true);
            HUDManager.instance.instructionContentCG
                .DOFade(1, .75f)
                .SetUpdate(true);
        });
    }

    public void HideInstruction()
    {
        Time.timeScale = 1;
        HUDManager.instance.instructionContentCG
            .DOFade(1, .75f).OnComplete(() =>
        {
            HUDManager.instance.instructionContent.SetActive(false);
            HUDManager.instance.instructionBGRectTransform
                .DOSizeDelta(new Vector2(0, HUDManager.instance.instructionBGRectTransform.sizeDelta.y), .5f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
            {
                HUDManager.instance.instructionHUD.SetActive(false);

                //ENABLE SCRIPT
                PlayerScript.instance.playerMovement.enabled = true;
                // PlayerScript.instance.playerMovement.playerAnim.enabled = true;
                PlayerScript.instance.cinemachineInputProvider.enabled = true;
                PlayerScript.instance.interact.enabled = true;
                // PlayerScript.instance.examine.enabled = true;

                // Cursor.lockState = CursorLockMode.Locked;

                HUDManager.instance.playerHUD.SetActive(true);
                // DisplayMission();
            });
        });
    }
}
