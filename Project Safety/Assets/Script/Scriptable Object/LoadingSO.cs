using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Scriptable Object/Loading")]
public class LoadingSO : ScriptableObject
{
    [Header("Did you know/Trivia/Facts")]
    [TextArea(3, 10)]
    public string[] loadingText;
    public Sprite[] previewScene;
    
}
