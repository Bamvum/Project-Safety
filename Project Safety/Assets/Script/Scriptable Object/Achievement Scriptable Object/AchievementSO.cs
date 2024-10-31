using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Achievement")]
public class AchievementSO : ScriptableObject
{
    public Sprite achievementSprite;
    public string achievementName;
    [TextArea(3, 5)]
    public string achievementDescription;
    public string achievementPlayerPrefsKey;
}
