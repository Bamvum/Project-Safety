using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] PlayerMovement playerMovement;
    
    [Header("Stamina")]
    [SerializeField] Image staminaBar;
    [SerializeField] float stamina = 100;
    [SerializeField] float maxStamina = 100;
    [SerializeField] float staminaCost;
    [SerializeField] float staminaRestoreRate;
    public bool outOfStamina;
    Coroutine staminaRecharge;

    void Awake()
    {
        
    }

    void Update()
    {
        if(playerMovement.runInput && !playerMovement.crouchInput && !outOfStamina)
        {
            stamina -= staminaCost * Time.deltaTime;
            staminaBar.fillAmount = stamina/maxStamina;

            if(stamina <= 0)
            {
                stamina = 0;
                outOfStamina = true;
            }
            
            if (staminaRecharge != null) 
            {
                StopCoroutine(staminaRecharge);
            }
            
            staminaRecharge = StartCoroutine(RechargeStamina());
        }           
    } 

    IEnumerator RechargeStamina()
    {
        if (stamina <= 0)
        {
            Debug.Log("Yield for 3 secs!");
            yield return new WaitForSeconds(3);
        }
        else
        {
            Debug.Log("Yield for 1 secs!");
            yield return new WaitForSeconds(1);
        }

        while (stamina < maxStamina)
        {
            stamina += staminaRestoreRate / 10f;

            if (stamina > maxStamina)
            {
                stamina = maxStamina;
            }

            if (stamina >= maxStamina)
            {
                outOfStamina = false;
            }

            staminaBar.fillAmount = stamina / maxStamina;

            yield return new WaitForSeconds(.1f);
        }
    }
}
