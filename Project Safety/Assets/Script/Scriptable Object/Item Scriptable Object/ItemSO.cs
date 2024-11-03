using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Item")]
public class ItemSO : ScriptableObject
{
    [Header("UI")]
    public string itemName;
    [TextArea(3, 5)]
    public string englishItemDescription;
    [TextArea(3, 5)]
    public string tagalogItemDescription;

    [Header("Examine")]
    [Range(0, 5)]
    public float itemDistanceToPlayer;
    [Range(-1, 1)]
    public float itemYoffset;

    [Header("Flags")]
    public bool isTakable;
    public bool isReadble;
}
