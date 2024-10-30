using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Achievement")]
public class AchievementSO : MonoBehaviour
{
    public Sprite achievementSprite;
    public string achievementName;
    public string achievementPlayerPrefsKey;
}
