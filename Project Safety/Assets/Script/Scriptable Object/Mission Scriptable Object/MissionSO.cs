using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Mission")]
public class MissionSO : ScriptableObject
{
    [Header("Mission")]
    public string[] missions;

    [Header("SFX")]
    public AudioClip missionCompleted;
}
