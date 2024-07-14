using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] RectTransform image;
    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = image.anchoredPosition;
        Debug.Log(startingPos);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
