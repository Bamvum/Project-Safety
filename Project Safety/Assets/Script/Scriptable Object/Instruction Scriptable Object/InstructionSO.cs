using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Object/Instructions")]
public class InstructionSO : ScriptableObject
{
    public List<InstructionProperties> instructions;
}

[System.Serializable]
public class InstructionProperties
{
    [SerializeField] string title;
    public Sprite instructionSprite;
    [TextArea(3, 10)]
    public string instructionString;
}