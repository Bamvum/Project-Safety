using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AimFireExtinguisher : MonoBehaviour
{
    [Header("Script")]
    [SerializeField] TPASS tpass;

    [Header("HUD")]
    [SerializeField] GameObject aimHUD;
    [SerializeField] RectTransform aimRectTransform;

    [Header("Interact")]
    [SerializeField] float interactRange;
    [SerializeField] RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        tpass.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (tpass.equipFireExtinguisher && tpass.twistDone && tpass.pullDone)
        {
            Debug.Log("Equip Fire Extinguiser, twist done, pull done");
            if (hit.collider != null)
            {
                // Image status hud
            }

            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            if (Physics.Raycast(ray, out hit, interactRange))
            {
                if (hit.collider.gameObject.layer == 9)
                {
                    // hit collided with fire;
                    Debug.Log("colided fire!");
                }

            }
        }
    }
}
